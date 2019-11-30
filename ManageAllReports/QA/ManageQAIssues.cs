using System;
using System.Data;
using System.IO;
using TNCSCAPI.Controllers.Reports.QuantityAccount;

namespace TNCSCAPI.ManageAllReports.QA
{
    public class ManageQAIssues
    {
        ManageReport report = new ManageReport();
        public void GenerateQAIssues(DataSet ds, QuantityAccountEntity quantityAccount)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = quantityAccount.GCode + GlobalVariable.QuantityAccountIssues;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + quantityAccount.UserId; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForReceipt(streamWriter, quantityAccount);
                AddDetails(streamWriter, ds);
                //AddFooter(streamWriter, chequeEntity);
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
        public void AddDocHeaderForReceipt(StreamWriter streamWriter, QuantityAccountEntity quantityAccount)
        {
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("                          TAMILNADU CIVIL SUPPLIES CORPORATION ");
            streamWriter.Write("       ");
            streamWriter.Write(report.StringFormatWithoutPipe(quantityAccount.RName, 30, 1));
            streamWriter.Write(report.StringFormatWithoutPipe("- Issues Abstract -", 22, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(quantityAccount.GName, 30, 2));
            streamWriter.WriteLine("");
            streamWriter.Write(" Date From.: "); //"R00002                             DATE: 03/Jan/2011");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(quantityAccount.FromDate), 13, 2));
            streamWriter.Write(" To : ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(quantityAccount.ToDate), 13, 2));
            streamWriter.WriteLine(" ");

        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddDetails(StreamWriter streamWriter, DataSet dataSet)
        {
            int i = 0;
            //Add header values
            int count = dataSet.Tables[0].Columns.Count;
            int length = (count * 20) + count + 18;
            streamWriter.WriteLine("-" + report.AddLine(length));
            streamWriter.Write(" ");
            foreach (DataColumn dataColumn in dataSet.Tables[0].Columns)
            {
                streamWriter.Write(report.StringFormat(dataColumn.ColumnName, i == 0 ? 20 : 20, i <= 1 ? 2 : 1));
                i ++;
            }
            streamWriter.Write("         Total    |");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("-" + report.AddLine(length));
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                i = 0;
                decimal Total = 0;
                streamWriter.Write(" ");
                for (int k = 0; k < count; k++)
                {
                    var result = report.StringFormatWithEmpty(Convert.ToString(item[k]));
                    if(i<=1)
                    {
                        streamWriter.Write(report.StringFormat(result.Item1, i == 0 ? 20 : 20, i <= 1 ? 2 : 1));

                    }
                    else 
                    {
                        Total = Total + (result.Item2 == true ? Convert.ToDecimal(result.Item1) : 0);
                        streamWriter.Write(report.StringFormat(report.DecimalformatForWeight(Convert.ToString(Total)), i == 0 ? 20 : 20, i <= 1 ? 2 : 1));
                    }
                    i++;
                }
                //Total
                streamWriter.Write(report.StringFormat(report.DecimalformatForWeight(Convert.ToString(Total)), 18, 1));
                streamWriter.WriteLine(" ");
            }
            streamWriter.WriteLine("-" + report.AddLine(length));
            streamWriter.WriteLine((char)12);
        }
    }
}