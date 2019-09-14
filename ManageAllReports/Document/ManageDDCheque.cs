using System;
using System.IO;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDDCheque
    {
        ManageReport report = new ManageReport();
        public void GenerateDDCheque(DDChequeEntity chequeEntity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = chequeEntity.GCode + GlobalVariable.DDChequeFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + chequeEntity.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForReceipt(streamWriter, chequeEntity);
                AddDetails(streamWriter, chequeEntity);
                AddFooter(streamWriter, chequeEntity);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                streamWriter.Flush();
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
        }

        /// <summary>
        /// Add header for document receipt
        /// </summary>
        /// <param name="streamWriter">Stream writer to write the text file.</param>
        /// <param name="stockReceipt"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForReceipt(StreamWriter streamWriter, DDChequeEntity chequeEntity)
        {
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("          TAMILNADU CIVIL SUPPLIES CORPORATION ");
            streamWriter.Write("       ");
            streamWriter.Write(report.StringFormatWithoutPipe(chequeEntity.RegionName, 30, 1));
            streamWriter.Write(report.StringFormatWithoutPipe("- RECEIPT -", 11, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(chequeEntity.GodownName, 30, 2));
            streamWriter.WriteLine("");
            streamWriter.Write(" RECP.NO.: "); //"R00002                             DATE: 03/Jan/2011");
            streamWriter.Write(report.StringFormatWithoutPipe(chequeEntity.ReceiptNo, 25, 2));
            streamWriter.Write("   ");
            streamWriter.Write("DATE : ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(chequeEntity.DDChequeItems[0].ReceiptDate), 13, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("------------------------------------------------------------------------------");
            streamWriter.Write("   Received with Thanks from Thiru / M/s.");
            streamWriter.Write(report.StringFormatWithoutPipe(chequeEntity.DDChequeItems[0].ReceivedFrom, 35, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("");
            streamWriter.Write(" Rupees ");
            string Rupees = ConvertNumbertoWords(Convert.ToInt64(chequeEntity.Total));
            Rupees = Rupees != "ZERO" ? Rupees + " ONLY" : Rupees;
            streamWriter.Write(report.StringFormatWithoutPipe(Rupees, 68, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine(" by Cash/DD/Cheque ");
            streamWriter.WriteLine("------------------------------------------------------------------------------");
            streamWriter.WriteLine("DD/Cheque No.      Date               Bank                      Amount ");
            streamWriter.WriteLine("------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddDetails(StreamWriter streamWriter, DDChequeEntity chequeEntity)
        {
            int i = 0;
            foreach (var item in chequeEntity.DDChequeItems)
            {
                i = i + 1;
                streamWriter.Write(report.StringFormat(item.PaymentType + " " + item.ChequeNo + " ", 16, 2));
                streamWriter.Write(report.StringFormat(report.FormatIndianDate(item.ChequeDate) + " ", 10, 2));
                streamWriter.Write(report.StringFormat(item.Bank + " ", 30, 2));
                streamWriter.Write(report.StringFormat(item.Amount, 14, 1));
                streamWriter.WriteLine(" ");
            }

        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddFooter(StreamWriter streamWriter, DDChequeEntity chequeEntity)
        {
            streamWriter.WriteLine("------------------------------------------------------------------------------");
            streamWriter.Write("                                                    Total |");
            streamWriter.Write(report.StringFormat(chequeEntity.Total, 14, 1));
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("------------------------------------------------------------------------------");
            streamWriter.WriteLine(report.StringFormatWithoutPipe(chequeEntity.Details, 55, 2));
            report.AddMoreContent(streamWriter, chequeEntity.Details, 55, 1);//Add content next line.
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("                                     For Tamil Nadu Civil Supplies Corporation");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("                                        Asst./ Cashier  Supdt. D.M A/c ");
            streamWriter.Write("                                    ");
            streamWriter.Write(report.StringFormatWithoutPipe(chequeEntity.RegionName, 30, 1));
            streamWriter.WriteLine();
            streamWriter.WriteLine((char)12);

        }

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = string.Empty;
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

    }
}
