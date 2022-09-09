using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Equiring_Application
{
    /* Данный класс нужен для проверки ответа от терминала */
    public class TerminalAnswerCheck
    {

       String amount, merchant_Category_Code, transnCurrencyCode;

        /* Инициализация конструктора класса TerminalAnswerCheck для проверка ответа от терминала*/
        public TerminalAnswerCheck(XmlDocument newXMLAnswer)
        {
            XmlAttributeCollection xmlCollection;
            foreach (XmlElement el in newXMLAnswer.GetElementsByTagName("tag"))
            {
                if (el.InnerText != "")
                {
                    /*Поиск аттрибутов тега*/
                    xmlCollection = el.Attributes;
                    
                    if (xmlCollection["name"].Value == "9F02")/* Amount, numeric*/
                    {
                        amount = el.InnerText;
                    }
                    else if (xmlCollection["name"].Value == "5F2A")/* Transaction Currency Code*/
                    {
                        transnCurrencyCode = el.InnerText;
                    }
                    else if (xmlCollection["name"].Value == "9F15")/* Merchant Category Code */
                    {
                        merchant_Category_Code = el.InnerText;
                    }
                }

            }
        }

        
        /* Проверка того, что все свойства не пусты */
        public bool allFieldsNotNull_or_Empty()
        {
            if (amount != null && amount != "" && merchant_Category_Code != null && merchant_Category_Code != "" && transnCurrencyCode != null && transnCurrencyCode != "")
                return true;

            return false;
        
        }


        /* Возвращает кол-во валюты */
        public string getAmount() 
        {
            if (amount != null)
                return amount;

            return "";
        }


        /* Возвращает код банка-продавца */
        public string getMerchantCode()
        {
            if (merchant_Category_Code != null)
                return merchant_Category_Code;

            return "";
        }


        /* Возвращает код валюты*/
        public string getCurrencyCode()
        {
            if (transnCurrencyCode != null)
                return transnCurrencyCode;

            return "";

        }

        /* Очистка всех свойств класса*/
        public void clearAllFields()
        {
            amount = "";
            merchant_Category_Code = "";
            transnCurrencyCode = "";
        }
    }
}
