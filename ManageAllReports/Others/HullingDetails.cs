using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class HullingDetails
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
        public void GenerateHullingReport(CommonEntity entity)
        {
            AuditLog.WriteError("GenerateHullingReport");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.HullingDetailsReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
               // DateWiseStockReceiptRegister(streamWriter, entity);

                List<HullingReportEntity> hullingReportList = new List<HullingReportEntity>();
                hullingReportList = report.ConvertDataTableToList<HullingReportEntity>(entity.dataSet.Tables[0]);

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
            sw.WriteLine("                               TAMILNADU CIVIL SUPPLIES CORPORATION      Region: " + RName  +      "Report Date : " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Hulling Rice Receipt Details of"  + GName + " Godown ");
            sw.WriteLine(" ");
            sw.WriteLine("          From:" + report.FormatDate(date) + "   To:" + report.FormatDate(date) + " Weight in Kilo Grams       Page No: 1");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|  Ack No          |Date            | Hulling Name                        |  Commodity               |   Bags     | Net Weight        |");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |                  |                |                                     |                          |            |                   |");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractDetails(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                   Hulling Rice Receipt Abstract Details of Godown: " + GName +   " Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
          //  sw.WriteLine("          From:" + report.FormatDate(date) + "   To:" + report.FormatDate(date) + " Weight in Kilo Grams       Page No: 1");
            sw.WriteLine("Abstract:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "       Region :" + RName);
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("Hulling Name                |Commodity                |  Bags      |Net Wt (in Kgs)/Nos |");
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------|");
        }
    }

    public class HullingReportEntity
    {
        public string AckNo { get; set; }
        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Depositor { get; set; }
        public string Bags { get; set; }
        public string NetWt { get; set; }
    }
}
