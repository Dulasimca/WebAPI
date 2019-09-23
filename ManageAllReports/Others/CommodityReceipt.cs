using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class CommodityReceipt
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
        public void GenerateCommodityReceiptReport(CommonEntity entity)
        {
            AuditLog.WriteError("GenerateCommodityReceiptReport");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.CommodityReceiptReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseCommodityReceiptReport(streamWriter, entity);

                List<CommodityReceiptList> commodityReceiptList = new List<CommodityReceiptList>();
                commodityReceiptList = report.ConvertDataTableToList<CommodityReceiptList>(entity.dataSet.Tables[0]);
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
            sw.WriteLine("                                    TAMILNADU CIVIL SUPPLIES CORPORATION                       " + RName);
            sw.WriteLine(" ");
            sw.WriteLine("                                        Receipt Datewise Details of - Commodity    Godown : " + GName);
            sw.WriteLine(" ");
            sw.WriteLine("          From:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate) + "          -EXCESS ");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|  Ack No   |Date      |   Commodity    |  Bags  |  Qty(Kgs)/NO's  |   Received From     | Lorry No  |   T.MEMO.NO   | T.MEMO DT |  ORDERNO  | EXCESS | SHORT |");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |           |          |                |        |                 |                     |           |               |           |           |        |       |");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseCommodityReceiptReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 10;
            var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Date");
            int i = 1;
            string ackNo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool CheckRepeatValue = false;
            bool isDataAvailable = false;
            foreach (DataRow date in dateList.Rows)
            {
                isDataAvailable = true;
                count = 11;
                string ackNoNext = string.Empty;
                AddHeader(sw, entity);
                DataRow[] data = entity.dataSet.Tables[0].Select("Date='" + date["Date"] + "'");
                foreach (DataRow row in data)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 11;
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeader(sw, entity);
                    }
                    ackNoNext = row["Ackno"].ToString();
                    fromWhomRcd = Convert.ToString(row["RecdFrom"]).Trim();
                    if (ackNo == ackNoNext)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        ackNo = ackNoNext;
                    }
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? i.ToString() : " ", 4, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? ackNoNext : " ", 11, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Date"].ToString() : " ", 10, 1));
                    sw.Write(report.StringFormat(row["Commodity"].ToString(), 16, 2));
                    sw.Write(report.StringFormat(row["Bags_No"].ToString(), 8, 1));
                    sw.Write(report.StringFormat(row["Quantity"].ToString(), 17, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? fromWhomRcd : " ", 21, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Lorryno"].ToString() : " ", 11, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["TruckMemoNo"].ToString() : " ", 15, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Truckmemodate"].ToString() : " ", 11, 1));
                    sw.Write(report.StringFormat(row["Orderno"].ToString(), 11, 2));
                    sw.Write(report.StringFormat("", 8, 1));
                    sw.Write(report.StringFormat("", 7, 1));
                    sw.WriteLine("");
                    i = CheckRepeatValue == false ? i + 1 : i;
                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            if (!isDataAvailable)
            {
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }

        }

    }

    public class CommodityReceiptList
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Scheme { get; set; }
        public string Ackno { get; set; }
        public DateTime Date { get; set; }
        public int Bags_No { get; set; }
        public string Commodity { get; set; }
        public string TruckMemoNo { get; set; }
        public double Quantity { get; set; }
        public string Lorryno { get; set; }
        public DateTime Truckmemodate { get; set; }
        public string Orderno { get; set; }
        public string RecdFrom { get; set; }
    }

}
