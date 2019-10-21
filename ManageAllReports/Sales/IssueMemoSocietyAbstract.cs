using System;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports.Sales
{
    public class IssueMemoSocietyAbstract
    {
        ManageReport report = new ManageReport();
        public void GenerateIssueMemoReceipt(CommonEntity commonEntity, string GlobalFileName, string reportName, int reportType)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = commonEntity.GCode + GlobalFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + commonEntity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForReceipt(streamWriter, commonEntity, reportName);
                AddDetails(streamWriter, commonEntity.dataSet, reportType);
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
        public void AddDocHeaderForReceipt(StreamWriter streamWriter, CommonEntity commonEntity, string reportName)
        {
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("                          TAMILNADU CIVIL SUPPLIES CORPORATION ");
            streamWriter.Write("       ");
            streamWriter.Write(report.StringFormatWithoutPipe(commonEntity.RName, 30, 1));
            streamWriter.Write(report.StringFormatWithoutPipe(reportName, 22, 2));
            streamWriter.Write(report.StringFormatWithoutPipe(commonEntity.GName, 30, 2));
            streamWriter.WriteLine("");
            streamWriter.Write(" Date From.: "); //"R00002                             DATE: 03/Jan/2011");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(commonEntity.FromDate), 13, 2));
            streamWriter.Write(" To : ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(commonEntity.Todate), 13, 2));
            streamWriter.WriteLine(" ");

        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddDetails(StreamWriter streamWriter, DataSet dataSet, int reportType)
        {
            int i = 0;
            //Add header values
            int count = dataSet.Tables[0].Columns.Count;
            int length = 0;
            length =  (count * 16) + count;
            streamWriter.WriteLine("---" + report.AddLine(length));
            streamWriter.Write(" ");
            int vad = (reportType == 0 && i < 2) || ((reportType == 1 && i < 3)) ? 2 : 1;
            foreach (DataColumn dataColumn in dataSet.Tables[0].Columns)
            {
                streamWriter.Write(report.StringFormat(dataColumn.ColumnName, 16, (reportType == 0 && i < 2) || ((reportType == 1 && i < 3)) ? 2 : 1));
                i++;
            }
            streamWriter.Write("       Total  |");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("---" + report.AddLine(length));
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                i = 0;
                decimal Total = 0;
                streamWriter.Write(" ");
                for (int k = 0; k < count; k++)
                {
                    var result = report.StringFormatWithEmpty(Convert.ToString(item[k]));
                    streamWriter.Write(report.StringFormat(result.Item1, 16, (reportType == 0 && i < 2) || ((reportType == 1 && i < 3)) ? 2 : 1));
                    if (i > 0)
                    {
                        Total = Total + (result.Item2 == true ? Convert.ToDecimal(result.Item1) : 0);
                    }
                    i++;
                }
                //Total
                streamWriter.Write(report.StringFormat(Convert.ToString(Total), 15, 1));
                streamWriter.WriteLine(" ");
            }
            streamWriter.WriteLine("---" + report.AddLine(length));
        }
    }
}
