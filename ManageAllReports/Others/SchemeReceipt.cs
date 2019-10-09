using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class SchemeReceipt
    {
        private string GName { get; set; }
        private string RName { get; set; }
        private string SCName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateSchemeReceipt(CommonEntity entity)
        {
            //AuditLog.WriteError("GenerateSchemeReceipt");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                SCName= entity.dataSet.Tables[0].Rows[0]["Scheme"].ToString();
                fileName = entity.GCode + GlobalVariable.SchemeReceiptReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseSchemeReceiptReport(streamWriter, entity);

                List<SchemeReceiptList> stockReceiptList = new List<SchemeReceiptList>();
                stockReceiptList = report.ConvertDataTableToList<SchemeReceiptList>(entity.dataSet.Tables[0]);
                StockIssuesCommoditywiseAbstract(streamWriter, stockReceiptList, entity);
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
        public void AddHeader(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("          TAMILNADU CIVIL SUPPLIES CORPORATION                       " + RName);
            sw.WriteLine(" ");
            sw.WriteLine("      Issue memo Date wise Details of " +  SCName  + " Scheme   Godown : " + GName);
            sw.WriteLine(" ");
            sw.WriteLine(" From:" + report.FormatDate(entity.FromDate) + "     To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("---------------------------------------------------------------------------------------------------");
            sw.WriteLine("S.No   Receipt.NO    Date          Commodity       Net Weight(Kgs)        Received From          ");
            sw.WriteLine("---------------------------------------------------------------------------------------------------");
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseSchemeReceiptReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 11;
            //  var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Date");
            int i = 1;
            string SINO = string.Empty;
            bool isDataAvailable = false;
            //foreach (DataRow date in dateList.Rows)
            //{
            isDataAvailable = true;
            AddHeader(sw, entity);
            // DataRow[] data = entity.dataSet.Tables[0].Select("Date='" + date["Date"] + "'");
            foreach (DataRow row in entity.dataSet.Tables[0].Rows)
            {
                if (count >= 50)
                {
                    //Add header again
                    count = 11;
                    sw.WriteLine("---------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddHeader(sw, entity);
                }
                sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                sw.Write(report.StringFormatWithoutPipe(row["Ackno"].ToString(), 14, 2));
                sw.Write(report.StringFormatWithoutPipe(row["Date"].ToString(), 10, 2));
                sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 16, 2));
                sw.Write(report.StringFormatWithoutPipe(row["Quantity"].ToString(), 20, 1));
                sw.Write(report.StringFormatWithoutPipe(Convert.ToString(row["RecdFrom"]).Trim(), 23, 2));
                sw.WriteLine("");
                sw.WriteLine(" ");
                i = i + 1;
                count++;
            }
            sw.WriteLine("---------------------------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
            //}
            if (!isDataAvailable)
            {
                sw.WriteLine("---------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="date"></param>
        public void AddAbstractHeader(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("          TAMILNADU CIVIL SUPPLIES CORPORATION                       " + RName);
            sw.WriteLine(" ");
            sw.WriteLine("      Issue memo Abstract Details of " + SCName + " Scheme   Godown : " + GName);
            sw.WriteLine(" ");
            sw.WriteLine(" From:" + report.FormatDate(entity.FromDate) + "     To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("--------------------------------------------------------");
            sw.WriteLine("    Commodity                    Net Weight(Kgs)   ");
            sw.WriteLine("--------------------------------------------------------");
        }

        /// <summary>
        /// Add Commodity wise abstract details 
        /// </summary>
        /// <param name="sw">Streamwriter</param>
        /// <param name="stockIssues">Stock issues entity</param>
        /// <param name="entity">Common entity</param>
        public void StockIssuesCommoditywiseAbstract(StreamWriter sw, List<SchemeReceiptList> stockReceipt, CommonEntity entity)
        {
            int count = 11;
            string weighmentType = string.Empty;
            var resultSet = from d in stockReceipt
                            group d by new { d.Commodity } into groupedData
                            select new
                            {
                                NetWt = groupedData.Sum(s => s.Quantity),
                                GroupByNames = groupedData.Key
                            };
            AddAbstractHeader(sw, entity);
            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("--------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddAbstractHeader(sw, entity);
                }
                sw.Write(" ");
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 31, 2));
                sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(item.NetWt.ToString()), 18, 2));
                sw.WriteLine(" ");
                count = count + 2;
            }
            sw.WriteLine("--------------------------------------------------------");
            sw.WriteLine((char)12);
        }

    }
    public class SchemeReceiptList
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Ackno { get; set; }
        public DateTime Date { get; set; }
        public string TruckMemoNo { get; set; }
        public string Lorryno { get; set; }
        public string Scheme { get; set; }
        public string Commodity { get; set; }
        public double Quantity { get; set; }
    }
}
