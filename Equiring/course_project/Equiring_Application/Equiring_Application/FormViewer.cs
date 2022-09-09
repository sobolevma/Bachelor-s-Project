using System.Windows.Forms;

namespace Equiring_Application
{
    /*Данный класс отвечает за инициализацию формы и обработку событий формы*/
    class FormViewer
    {
        /* Название всех контактных тестов */
        private string[] contactDescriptions = {
            "Basic VSDC 01", 
            "19-Digit Account Number 02",
            "T=1, DDA, Offline Enc.PIN, and Issuer Authentication 03",
            "Terminal Risk Management 04",
            "Application Selection 05",
            "Dual Interface 06",
            "Terminal Action Codes 07",
            "Fallback 08",
            "Reserved for Future Use - CVM 09",
            "CDA 10",
            "Multiple Applications 11",
            "Geographic Restrictions 12",
            "Proprietary Data and 6-Digit PIN 13",
            "Long PDOL and Unrecognized Tag 14",
            "Data Element with 2-Byte Length Field 15",
            "Two Applications and Cardholder Confirmation 16",
            "Magnetic Stripe Image 17",
            "T=1 and DDA with 1984 Certificate 18",
            "Plus and Visa Interlink 19",
            "Visa Electron 20",
            "Combination CVM and Visa Fleet Chip 23",
            "Account Number with Padded Fs 24",
            "No PAN Sequence Number 25", 
            "PAN Sequence Number of 11 26", 
            "1144-Bit Issuer Public Key 27",
            "Multiple Features 28",
            "Blocked Card 29"
        };

        /* Название всех бесконтактных тестов */
        private string[] contactlessDescription = {
            "VCPS 2.0.2 Baseline Card (online) 01_a",
            "VCPS 2.0.2 Baseline Card (offline) 01_b",
            "VCPS 2.1.2 Baseline Card (online) 02_a",
            "VCPS 2.1.2 Baseline Card (offline) 02_b",
            "Card with a 16-byte ADF Name and other Features (online) 03_a",
            "Card with a 16-byte ADF Name and other Features (offline) 03_b",
            "Card with additional Data in the GPO Response and with no Cardholder Name (online) 04_a",
            "Card with additional Data in the GPO Response and with no Cardholder Name (offline) 04_b",
            "Card that returns additional Data in the Select PPSE Response and containing other unique features (online) 05_a",
            "Card that returns additional Data in the Select PPSE Response and containing other unique features (offline) 05_b",
            "qVSDC-only card with 19-digit PAN and CVN 18 (online) 06_a",
            "qVSDC-only card with 19-digit PAN and CVN 18(offline) 06_b",
            "Online-only card with an Electron AID 07ab",
            "Card with an Interlink AID 08ab",
            "Card supporting MSD Legacy only 09ab",
            "VMPA Card containing (00) and (FF) Padding in the GPO and a Read Record Response Respectively 10ab",
            "Card with an IAD length of 23-bytes and an Unrecognized CVN (online) 11_a", 
            "Card with a VMPA Applet that causes a Pre-tap (online) 12_a", 
            "Card with a CTQ indicating an issuer preference to switch to contact interface on ODA failure (offline) 13_b",
            "Card that Declines Transactions 14_a",
            "Card that Declines Transactions (offline) 14_b",
            "Card with inconsistent Data (offline) 15_b",
            };


        /* Данный метод отвечает за добавление в таблицы заголовков */
        public void tableZagolovki(TableLayoutPanel tabelPanel, string[] zagolovki)
        {
            Label[] zagolovLabels = { new Label(), new Label(), new Label(), new Label() };

            for (int i = 0; i < zagolovki.Length; i++)
            {
                zagolovLabels[i].Text = zagolovki[i];
                tabelPanel.Controls.Add(zagolovLabels[i], i, 0);
            }
        }


        /* Данный метод отвечает за инициализация таблиц начальными данными */
        public void tableLayoutProcessing(TableLayoutPanel tabelPanel, string[] listOfFiles, string link)
        {
            string[] filenames = new string[listOfFiles.Length];
            string Testname;

            Label[] filenameLabels = new Label[listOfFiles.Length];
            Label nameLabel;

            for (int i = 0; i < filenames.Length; i++)
            {
                // Добавляем новую строку в таблицу
                Testname = getTestName(listOfFiles[i], link);
                              
                filenames[i] = listOfFiles[i];                
                filenames[i] = cutfile(filenames[i], link);

                filenameLabels[i] = new Label();
                filenameLabels[i].Text = filenames[i];
                nameLabel = new Label();
                nameLabel.Text = Testname;

                tabelPanel.Controls.Add(new CheckBox(), 0, i + 1);
                tabelPanel.Controls.Add(filenameLabels[i], 1, i + 1);
                tabelPanel.Controls.Add(nameLabel, 2, i + 1);

            }
        }


        /* Данный метод возвращает краткое имя теста*/
        private string getTestName(string fileLink, string firstPartOflink)
        {
            string testName = "";

            if (fileLink.Contains("Contactless"))
            {
                fileLink = cutfile(fileLink, firstPartOflink);
                testName = findDescription(fileLink, contactlessDescription);
            }
            else if (fileLink.Contains("Contact"))
            {
                fileLink = cutfile(fileLink, firstPartOflink);
                testName = findDescription(fileLink, contactDescriptions);
            }

            return testName;
        }


        /* Данный метод отвечает за преобразование полного имени теста в краткое имя*/
        private string cutfile(string name, string firstPartOftheLink)
        {
            string substr = "contact";
            name = name.Replace(firstPartOftheLink + "\\", "");
           
            name = name.Replace(substr, "");
            substr = "less";
            name = name.Replace(substr, "");
            substr = ".xml";
            name = name.Replace(substr, "");

            return name;
        }


        /* Данный метод возвращает описание теста и краткое имя теста */
        public string[] getTappedCheckboxTest(TableLayoutPanel tabelPanel)
        {
            string[] testNum = new string[2];
            for (int j = 1; j <= tabelPanel.RowCount; j++)
            {
                Control control1 = tabelPanel.GetControlFromPosition(0, j);
                if (control1 != null)
                {
                    CheckBox checkBox = (CheckBox)control1;
                    if (checkBox.Checked)
                    {
                        // Получение краткого имени теста
                        Control controlTest = tabelPanel.GetControlFromPosition(1, j);
                        Label testLabelName = (Label)controlTest;
                        testNum[0] = testLabelName.Text;

                        // Получение описания теста
                        Control controlDescriptTest = tabelPanel.GetControlFromPosition(2, j);
                        Label testDescriptionName = (Label)controlDescriptTest;
                        testNum[1] = testDescriptionName.Text;

                        break;
                    }


                }
            }
            return testNum;
        }


        /* Данный метод возвращает описание теста */
        private string findDescription(string testName, string[] testDescriptions)
        {
            for (int i = 0; i < testDescriptions.Length; i++)
            {
                if (testDescriptions[i].Contains(testName))
                {
                    string returnedDescription = testDescriptions[i];
                    returnedDescription = returnedDescription.Replace(" " + testName, "");
                    return returnedDescription;
                }

            }
            return "";
        }


        /* Данный метод отвечает за сброс всех выбранных Checkbox */
        public void allSelectedtoDeselected(TableLayoutPanel tabelPanel)
        {
            for (int j = 1; j <= tabelPanel.RowCount; j++)
            {
                Control control1 = tabelPanel.GetControlFromPosition(0, j);
                
                if (control1 != null && control1.ToString().Contains("Label") == false)
                {
                    CheckBox checkBox = (CheckBox)control1;
                    if (checkBox.Checked)
                    {
                        checkBox.Checked = false;
                        tabelPanel.Controls.Add(checkBox, 0, j);
                        
                    }
                }
            }
        }
    }
}
