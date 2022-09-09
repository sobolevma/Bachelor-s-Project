using System.Xml;


namespace Equiring_Application
{
    /* Данный класс отвечает за создание XML-ответов на сообщения терминала и добавляет конфигурации необходимые тэги*/
    class TCPStrings
    {
        /* Данный метод добавляет файлу конфигурации необходимые тэги */
        public XmlDocument createXMLForTransmission(XmlDocument someXMLConfiguration)
        {
            XmlDocument fullXMLConfiguration = new XmlDocument();
            XmlElement loadEMVConfiguration = fullXMLConfiguration.CreateElement("loademvconfiguration");
            fullXMLConfiguration.AppendChild(loadEMVConfiguration);
            XmlElement emv = fullXMLConfiguration.CreateElement("emv");
            loadEMVConfiguration.AppendChild(emv);
            XmlElement xRoot = someXMLConfiguration.DocumentElement;
            XmlNode fullXMLNode;

            //MessageBox.Show(xRoot.OuterXml);
            fullXMLNode = fullXMLConfiguration.ImportNode(xRoot, true);
            emv.AppendChild(fullXMLNode);

            return fullXMLConfiguration;
        }


        /* Данный метод создает xml-файл, с которым будет сравниваться ответ терминала на отправку конфигурации */
        public XmlDocument createXmlConfAnswer()
        {
            XmlDocument answerXML = new XmlDocument();
            XmlElement loadEMVConfiguration = answerXML.CreateElement("loademvconfiguration");
            answerXML.AppendChild(loadEMVConfiguration);
            XmlElement status = answerXML.CreateElement("status");
            XmlText text = answerXML.CreateTextNode("ok");
            loadEMVConfiguration.AppendChild(status);
            status.AppendChild(text);

            return answerXML;
        }


        /* Данный метод создает keepalive-ответ для терминала */
        public XmlDocument keepaliveConfAnswer()
        {
            XmlDocument answerXML = new XmlDocument();
            XmlElement keepalive = answerXML.CreateElement("keepalive");
            answerXML.AppendChild(keepalive);
            XmlElement status = answerXML.CreateElement("status");
            XmlText text = answerXML.CreateTextNode("ok");
            keepalive.AppendChild(status);
            status.AppendChild(text);

            return answerXML;
        }

    }
}
