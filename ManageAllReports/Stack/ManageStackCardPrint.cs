using System;
using System.Collections.Generic;
using System.IO;
using TNCSCAPI.Controllers.Reports.Stack;

namespace TNCSCAPI.ManageAllReports.Stack
{
    public class ManageStackCardPrint
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockIssuesEntity"></param>
        public void GenerateStackCard(List<StackCardEntity> stackCardEntities, StackEntity stackEntity)
        {
            // AuditLog.WriteError("GeneratestockIssuesEntityRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = stackEntity.GCode + GlobalVariable.StackCardFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + stackEntity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForIssues(streamWriter, stackCardEntities, stackEntity);
                AddDetails(streamWriter, stackCardEntities);

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
        /// <param name="stockIssuesEntity"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForIssues(StreamWriter streamWriter, List<StackCardEntity> stackCardEntities, StackEntity stackEntity)
        {
            streamWriter.WriteLine("                                                                           ");
            streamWriter.WriteLine("               TAMILNADU CIVIL SUPPLIES CORPORATION                        ");
            streamWriter.Write("                ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.RName, 45, 2));
            streamWriter.WriteLine("");
            streamWriter.Write("  Commodity: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.ITName, 25, 2));
            streamWriter.Write("  Godown :");
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.GName, 25, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("                                             Stack.No: " + stackEntity.TStockNo);
            streamWriter.WriteLine("----|--------------------------------|-----------------------------------------|");
            streamWriter.WriteLine("    |        RECEIPT                 |              ISSUES                     |");
            streamWriter.WriteLine("----|--------------------------------|-----------------------------------------|");
            streamWriter.WriteLine("Slno|  DATE      Bags.   Quantity    |   Bags.     Quantity         Clo.Bal.   |");
            streamWriter.WriteLine("----|--------------------------------|-----------------------------------------|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddDetails(StreamWriter streamWriter, List<StackCardEntity> stackCardEntities)
        {
            int i = 1;
            string Fromdate = string.Empty;
            string Todate = string.Empty;
            string TotalReceiptBags = string.Empty, TotalIssuesBags = string.Empty;
            string TotalReceiptQuantity = string.Empty, TotalIssuesQuantity = string.Empty;
            foreach (var item in stackCardEntities)
            {

                if (i == 1)
                {
                    Fromdate = item.AckDate;
                }

                if (item.AckDate == "Total")
                {
                    streamWriter.WriteLine("----|--------------------------------|-----------------------------------------|");
                    streamWriter.Write(report.StringFormatWithoutPipe(item.AckDate, 15, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.ReceiptBags).Item1, 5, 1));
                    streamWriter.Write(report.StringFormat(report.StringFormatWithEmpty(item.ReceiptQuantity).Item1, 15, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.IssuesBags).Item1, 5, 2));
                    streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.IssuesQuantity).Item1, 15, 1));
                    streamWriter.Write(report.StringFormat(report.StringFormatWithEmpty(item.ClosingBalance).Item1, 19, 1));
                    streamWriter.WriteLine(" ");
                    streamWriter.WriteLine("-------------------------------------------------------------------------------|");
                    TotalReceiptBags = item.ReceiptBags;
                    TotalIssuesBags = item.IssuesBags;
                    TotalReceiptQuantity = item.ReceiptQuantity;
                    TotalIssuesQuantity = item.IssuesQuantity;
                    break;
                }
                Todate = item.AckDate;
                streamWriter.Write(report.StringFormat(i.ToString(), 4, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(item.AckDate), 10, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.ReceiptBags).Item1, 5, 1));
                streamWriter.Write(report.StringFormat(report.StringFormatWithEmpty(item.ReceiptQuantity).Item1, 15, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.IssuesBags).Item1, 5, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(report.StringFormatWithEmpty(item.IssuesQuantity).Item1, 15, 1));
                streamWriter.Write(report.StringFormat(report.StringFormatWithEmpty(item.ClosingBalance).Item1, 19, 1));
                streamWriter.WriteLine(" ");

                i = i + 1;
            }
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("                                                Bags     Quantity");
            streamWriter.WriteLine("-------------------------------------------------------------------------------|");
            streamWriter.Write("                                  Total Receipt   ");  
            streamWriter.Write(report.StringFormatWithoutPipe(TotalReceiptBags, 8, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(TotalReceiptQuantity, 13, 2));
            streamWriter.WriteLine("");
            streamWriter.Write("                                  Total Issues    ");  
            streamWriter.Write(report.StringFormatWithoutPipe(TotalIssuesBags, 8, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(TotalIssuesQuantity, 13, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("-------------------------------------------------------------------------------|");
            streamWriter.Write("                                       Balance    ");  
            streamWriter.Write(report.StringFormatWithoutPipe(GetDifference(TotalReceiptBags, TotalIssuesBags, 1), 8, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(GetDifference(TotalReceiptQuantity, TotalIssuesQuantity, 2), 13, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("-------------------------------------------------------------------------------|");
            streamWriter.WriteLine("-------------------------------------------------------------------------------|");
            streamWriter.WriteLine("                                  FROM DATE    TODATE     TOTAL DAYS");
            streamWriter.Write("                Period of Storage ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(Fromdate), 12, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(Todate), 12, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(report.GetDays(Fromdate,Todate), 12, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("-------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string GetDifference(string firstValue, string secondValue, int Type)
        {
            string result = string.Empty;

            try
            {
                if (Type == 1)
                {
                    result = Convert.ToString(Convert.ToInt32(firstValue) - Convert.ToInt32(secondValue));
                }
                else
                {
                    result = (Convert.ToDecimal(firstValue) - Convert.ToDecimal(secondValue)).ToString("N3");
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetDifference : " + ex.Message);
            }

            return result;
        }
    }
}
