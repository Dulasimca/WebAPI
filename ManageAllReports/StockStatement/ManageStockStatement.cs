using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class ManageStockStatement
    {
        private string GName { get; set; }
        private string RName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateStockStatementReport(List<DailyStockDetailsEntity> stockDetailsEntity, StockParameter param)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = (param.GCode + GlobalVariable.StockStatementFileName);
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + param.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                StockStatementReport(streamWriter, stockDetailsEntity, param);

                //List<DailyStockDetailsEntity> dailyStocks = new List<DailyStockDetailsEntity>();
                //dailyStocks = report.ConvertDataTableToList<DailyStockDetailsEntity>(dailyStocks);
                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="date"></param>
        public void AddHeader(StreamWriter sw, StockParameter entity, int pageNo)
        {
            sw.WriteLine("                                          TamilNadu Civil Supplies Corporation       " + entity.RName);
            sw.WriteLine("                                             Stock Statement Details of " + entity.GName + " Godown");
            sw.WriteLine("     From : " + report.FormatDate(entity.FDate) + "    To : " + report.FormatDate(entity.ToDate) +            "                                           Page No :" + pageNo.ToString());
            sw.WriteLine("|------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("|S.No| Commodity      |Ope.Bal      |Receipt      |Total(OB+Receipt)|Total Issue  |Closing.Bal  |Cummulative Shortage|Current CS   |Phy.Bal      |");
            sw.WriteLine("|------------------------------------------------------------------------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void StockStatementReport(StreamWriter sw, List<DailyStockDetailsEntity> stockDetailsEntity, StockParameter entity)
        {
            try
            {
                int count = 8;
                int pageNo = 1;
                AddHeader(sw, entity, pageNo);
                int i = 1;
                foreach (var data in stockDetailsEntity)
                {
                  
                        if (count >= 50)
                        {
                            //Add header again
                            pageNo++;
                            count = 8;
                        sw.WriteLine("|------------------------------------------------------------------------------------------------------------------------------------------------|");
                            sw.WriteLine((char)12);
                            AddHeader(sw, entity, pageNo);
                        }
                        sw.Write("|");
                        sw.Write(report.StringFormat(i.ToString(), 4, 2));
                        sw.Write(report.StringFormat(data.ITDescription.ToString(), 16, 2));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.OpeningBalance.ToString()), 13, 1));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.TotalReceipt.ToString()), 13, 1));
                        sw.Write(report.StringFormat((report.DecimalformatForWeight((data.TotalReceipt + data.OpeningBalance).ToString())), 17, 1));
                        sw.Write(report.StringFormat((report.DecimalformatForWeight((data.IssueSales + data.IssueOthers).ToString())), 13, 1));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.ClosingBalance.ToString()), 13, 1));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.CSBalance.ToString()), 20, 1));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.Shortage.ToString()), 13, 1));
                        sw.Write(report.StringFormat(report.DecimalformatForWeight(data.PhycialBalance.ToString()), 13, 1));
                        sw.WriteLine("");
                        count = count + 1;
                        i++;
                }
                sw.WriteLine("|------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
