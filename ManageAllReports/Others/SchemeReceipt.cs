using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class SchemeReceipt
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
        public void GenerateSchemeReceipt(CommonEntity entity)
        {
            AuditLog.WriteError("GenerateSchemeReceipt");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.SchemeReceiptReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
             //   DateWiseStockReceiptRegister(streamWriter, entity);

                List<SchemeReceiptList> schemeReceiptList = new List<SchemeReceiptList>();
                schemeReceiptList = report.ConvertDataTableToList<SchemeReceiptList>(entity.dataSet.Tables[0]);

                // DateWiseStockReceiptRegister(streamWriter, entity);
                //StockReceiptAbstractRecdTypeAndSchemeWise(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractSchemeAndCommodityWise(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractStackNoAndCommodity(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractCommodityWise(streamWriter, stockReceiptList, entity);

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
        public void AddHeader(StreamWriter sw, string date)
        {
            sw.WriteLine("                                  TAMILNADU CIVIL SUPPLIES CORPORATION                        Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                         Receipt Date Wise Details of BULK Scheme");
            sw.WriteLine(" ");
            sw.WriteLine("          From:" + report.FormatDate(date) + " To:" + report.FormatDate(date) + " Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.NO|  ACK.NO   |Date      | Commodity          |   NET Weight(kgs)   |   Scheme   |  Stack No  |No bags |   Commodity   |Net Weight|");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |           |                   |           |                                 |            |            |        |               |          |");
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
