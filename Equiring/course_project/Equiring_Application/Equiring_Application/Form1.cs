using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Windows.Forms;


namespace Equiring_Application
{
    public partial class Form1 : Form
    {
        /* Класс FormViewer отвечает за обработку событий формы*/
        FormViewer formViewer = new FormViewer();
        
        string[] listOfContactTestFiles, listOfContactlessTestFiles;
        string choosedFullTestName = "";

        /* Инициализация формы */
        public Form1()
        {
            InitializeComponent();
           
            string[] zagolovki = { "Выбор", "Тест", "Название теста" };
            formViewer.tableZagolovki(tableLayoutPanel1, zagolovki);
            formViewer.tableZagolovki(tableLayoutPanel2, zagolovki);

            // Относительная ссылка
            string partLink = @"../Tests";
            string link = Path.GetFullPath(partLink);
            

            listOfContactTestFiles = getFiles(link + "/Contact");
            formViewer.tableLayoutProcessing(tableLayoutPanel1, listOfContactTestFiles, link + "/Contact");

            listOfContactlessTestFiles = getFiles(link + "/Contactless");
            formViewer.tableLayoutProcessing(tableLayoutPanel2, listOfContactlessTestFiles, link + "/Contactless");
        }
        

        /* Загрузка конфигурации в терминал по нажатию на кнопку <Оплата> */
        private void button1_Click(object sender, EventArgs e)
        {
            TCPStrings TCPString = new TCPStrings();
            XmlDocument xmlConfiguration = new XmlDocument();
            
            if (choosedFullTestName != "" && choosedFullTestName != null) 
            {
                xmlConfiguration.Load(choosedFullTestName);
                XmlDocument fullConfiguration_ToTransmit = TCPString.createXMLForTransmission(xmlConfiguration);
                XmlDocument confAnswer = TCPString.createXmlConfAnswer();
                XmlDocument alivAnswer = TCPString.keepaliveConfAnswer();
                MessageBox.Show(fullConfiguration_ToTransmit.OuterXml);
                
                //Отправляем необходимые данные в обработчик TCP-запросов
                TCPConnection newConnection = new TCPConnection();
                string requestString = newConnection.transactionAnswerFromServer(fullConfiguration_ToTransmit, confAnswer, alivAnswer);
                MessageBox.Show("Получен результат работы транзакции от терминала.");


                requestString = "<answer><tag name = \"9F02\" >000000002511</tag> <tag name = \"5F2A\">0643</tag> <tag name=\"9F15\">0001</tag></answer>";
                /* Преобразуем строку ответа в XML*/
                XmlDocument requestXML = new XmlDocument();
                requestXML.LoadXml(requestString);

                /* Отправляем ответ терминала в виде XML на проверку*/
                TerminalAnswerCheck newAnswerCheck = new TerminalAnswerCheck(requestXML);

                /* Вызов новой формы при наличии всех необходимых заполненных полей в ответе терминала*/
               if(newAnswerCheck.allFieldsNotNull_or_Empty())
               {
                    /* Вызов формы "Результат обработки транзакции" в виде диалогового окна */
                    Form2 result_Form = new Form2(newAnswerCheck);
                    result_Form.ShowDialog(this);
                    result_Form.Dispose();
                }
               else 
                    MessageBox.Show("В ответе от терминала содержатся пустые поля, которые не должны быть пустыми.");

            }
            else
                MessageBox.Show("Не выбран ни один тест.");

            /* Сброс выбранного имени */
            choosedFullTestName = "";

        }


        /* Выгрузить помеченный галочкой тест по нажатию на кнопку <Выбрать> */
        private void button3_Click(object sender, EventArgs e)
        {
            // Получить часть имени файла с тестом
            string [] testParametres;
            string choosedPartTestName, testDescription;
            
            if (tabControl1.SelectedIndex == 0)// Если открыта страница ContactTests
            {
                testParametres = formViewer.getTappedCheckboxTest(tableLayoutPanel1);
                choosedPartTestName = testParametres[0];

                if (choosedPartTestName != "" && choosedPartTestName != null)
                {
                    // Получить полное имени файла с тестом
                    choosedFullTestName = retChoosedFoolTestName(choosedPartTestName, listOfContactTestFiles);
                    
                    testDescription = testParametres[1];
                    
                    MessageBox.Show("Описание теста: " + testDescription + ".\n\nВставьте карту в терминал для выполнения теста! \nИ затем нажмите кнопку <Оплатить>.");
                }                
            }
            else // Если открыта страница ContactlessTests
            {
                testParametres = formViewer.getTappedCheckboxTest(tableLayoutPanel2);
                choosedPartTestName = testParametres[0];

                if (choosedPartTestName != "" && choosedPartTestName != null)
                {
                    // Получить полное имени файла с тестом
                    choosedFullTestName = retChoosedFoolTestName(choosedPartTestName, listOfContactlessTestFiles);
                    
                    testDescription = testParametres[1];

                    MessageBox.Show("Описание теста: " + testDescription + ".\n\nПоднесите карту близко к терминалу! \nИ затем нажмите кнопку <Оплатить>.");                    
                }
            }

        }


        /* Получаем список всех файлов, полученных по заданой ссылке searchLink с расширением *.xml*/
        private string[] getFiles(string searchLink)
        {
            Reader reader = new Reader();
                       
            string[] listOfTestFiles = reader.SearchFile(searchLink, "*.xml");

            return listOfTestFiles;
        }
         
        
        /* Сброс всех выставленных галочек в TabPage1 */
        private void tabPage2_Click(object sender, EventArgs e)
        {
            formViewer.allSelectedtoDeselected(tableLayoutPanel1);
        }


        /* Возвращает полное имя файла */
        private string retChoosedFoolTestName(string partName, string[] listOfTestFiles) {

            // Поиск по контактным  либо бесконтактным файлам
            
            for (int i = 0; i < listOfTestFiles.Length; i++)
            {
                if (listOfTestFiles[i].Contains(partName))
                {
                         return listOfTestFiles[i];
                }
            }
          
            return "";
        }

    }
}
