using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Equiring_Application
{
    /* Данный класс отвечает за отправку и прием сообщений по TCP-соединению*/
    class TCPConnection
    {
        const string IP_ADDRESS = "192.168.0.100";
        const int PORT = 4433;
        const int TIMEOUT = 1000;

        /* Данный метод отвечает за создание TCP соединения и за отправку данных на терминал и возврат результата выполнения транзакции */
        public  string transactionAnswerFromServer(XmlDocument configuration, XmlDocument approvedConfiguration, XmlDocument keepalive_answer)
        {
            string approved_StringConf = approvedConfiguration.OuterXml;
            
            IPAddress address = IPAddress.Parse(IP_ADDRESS);
            
            Boolean contin = true;
            String transactionData = "";
           
            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(address, PORT);
                byte[] byteConf = Encoding.UTF8.GetBytes(File.ReadAllText(configuration.OuterXml));
                byte[] byte_keepalive = Encoding.UTF8.GetBytes(File.ReadAllText(keepalive_answer.OuterXml));
                // Get the stream used to read the message sent by the server.
                NetworkStream confStream = tcpClient.GetStream();
                try 
                {
                    // Отправка конфигурации в терминал
                    confStream.Write(byteConf, 0, byteConf.Length);
                   

                    while(contin)
                    {
                        // Выставляем timeout чтения
                        confStream.ReadTimeout = TIMEOUT;

                        // Получение ответа
                        String responseData = readStringFromServer(confStream); // Получаем ответ от сервера
                        
                        if(responseData.Contains("<keepalive/>")) // если получено сообщение <keepalive/>
                        {
                            confStream.Write(byte_keepalive, 0, byte_keepalive.Length);
                            transactionData = "";
                        }                            
                        else if (responseData.Contains(approved_StringConf))
                        {
                        
                            transactionData = readStringFromServer(confStream); // Считывание результата обработки транзакции
                            contin = false; 
                        }
                        else
                        {
                            transactionData = "";
                            contin = false;
                        }                    
                    }
                   
                        
                }
                finally
                {
                   /* Закрыть поток чтения и записи*/
                    confStream.Close();

                    /* Закрыть TCP-соединение*/
                    tcpClient.Close();
                }
                

            }
            catch (Exception ex) // если получено исключение
            {
                MessageBox.Show(ex.Message);
                transactionData = "";
            }
            return transactionData;
        }


        /* Данный метод обеспечивает чтение данных, поступающих с сервера */
        private static string readStringFromServer(NetworkStream networkStream)
        {
            Byte[] readingData = new Byte[1024];           
            StringBuilder completeMessage = new StringBuilder();
            int numberOfBytesRead = 0;
            do
            {
                numberOfBytesRead = networkStream.Read(readingData, 0, readingData.Length);
                completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
            }
            while (networkStream.DataAvailable);

            return completeMessage.ToString();
        }
    }

   
}
