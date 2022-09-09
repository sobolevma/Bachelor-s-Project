using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace Equiring_Application
{
    /*Инициализация формы "Результат обработки транзакции" */
    public partial class Form2 : Form
    {
        TerminalAnswerCheck answerCheck;

        public Form2(TerminalAnswerCheck terminal_Answer_Check)
        {
            InitializeComponent();
           
            /* Инициализация поля "Остаток на карте" */           
            textBox1.Text = terminal_Answer_Check.getAmount();

            /*Инициализация поля "Код валюты" */           
            textBox2.Text = terminal_Answer_Check.getCurrencyCode();
           
            if (terminal_Answer_Check.getCurrencyCode() == "0643")
                textBox4.Text = "руб.";

            /* Инициализация поля "Код продавца" */            
            textBox3.Text = terminal_Answer_Check.getMerchantCode();

            answerCheck = terminal_Answer_Check;
        }


        /* Обработка события нажатия на кнопку "Выйти". */
        private void button1_Click(object sender, EventArgs e)
        {
            answerCheck.clearAllFields();

            /* Закрытие формы */
            Close();
        }
    }
}
