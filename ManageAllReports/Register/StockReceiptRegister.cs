using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace TNCSCAPI.ManageAllReports
{
    public class StockReceiptRegister
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
        public void GenerateStockReceiptRegister(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.StockReceiptRegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseStockReceiptRegister(streamWriter, entity);

                List<StockReceiptList> stockReceiptList = new List<StockReceiptList>();
                stockReceiptList = report.ConvertDataTableToList<StockReceiptList>(entity.dataSet.Tables[0]);

                // DateWiseStockReceiptRegister(streamWriter, entity);
                StockReceiptAbstractRecdTypeAndSchemeWise(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractSchemeAndCommodityWise(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractStackNoAndCommodity(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractCommodityWise(streamWriter, stockReceiptList, entity);

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
            sw.WriteLine("                                          Stock Receipt Register");
            sw.WriteLine(" ");
            sw.WriteLine("          Stock Receipt Register:" + report.FormatDate(date) + "           Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|  Ack No   |Truck Memo No      | Lorry No  |   From Whom Received            |   Scheme   |  Stack No  |No bags |   Commodity   |Net Weight|");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |           |                   |           |                                 |            |            |        |               |          |");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractStackAndCommodity(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                    Stock Receipt Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Abstract:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "    Godown : " + GName + "       Region :" + RName);
            sw.WriteLine("--------------------------------------------------------------------------|");
            sw.WriteLine("StackNo       |Commodity               |No Bags      |Net Wt (in Kgs)/Nos |");
            sw.WriteLine("--------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractSchemeAndCommodityWise(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine(" Stock Receipt Register Scheme Wise Abstract    Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Stock Receipt Register:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "  Godown : " + GName + "  Region :" + RName);
            sw.WriteLine("------------------------------------------------------------------------------|");
            sw.WriteLine("Scheme             |Commodity             |No Bags       |Net Wt (Kgs/Nos)    |");
            sw.WriteLine("------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractRecTypeAndSchemeWise(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("     Stock Receipt Register Recd.Type + Scheme Wise Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Stock Receipt Register:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "    Godown : " + GName + "   Region :" + RName);
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("RECD.TYPE       |RECD.FROM      |Scheme         |Commodity            |No Bags     |Net Wt (Kgs/Nos)  |Packing Type         |");
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractCommodityWise(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("            Stock Receipt Register Commodity Wise Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Stock Receipt Register:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "    Godown : " + GName + "    Region :" + RName);
            sw.WriteLine("----------------------------------------------------------------------------------|");
            sw.WriteLine("Commodity              |Packing Type          |No Bags     |Net Wt (Kgs/Nos)      |");
            sw.WriteLine("----------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseStockReceiptRegister(StreamWriter sw, CommonEntity entity)
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
                DataRow[] data = entity.dataSet.Tables[0].Select("Date='" + date["Date"] + "'");
                AddHeader(sw, Convert.ToString(date["Date"]));
                foreach (DataRow row in data)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 11;
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeader(sw, Convert.ToString(date["Date"]));
                    }
                    ackNoNext = row["Ackno"].ToString();
                    fromWhomRcd = Convert.ToString(row["From_Whom_Received"]).Trim();
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
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? ackNoNext : " ", 11, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["TruckMemoNo"].ToString() : " ", 19, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Lorryno"].ToString() : " ", 11, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? fromWhomRcd : " ", 33, 2));
                    sw.Write(report.StringFormat(row["Scheme"].ToString(), 12, 2));
                    sw.Write(report.StringFormat(row["Stackno"].ToString(), 12, 2));
                    sw.Write(report.StringFormat(row["NoPacking"].ToString(), 8, 1));
                    sw.Write(report.StringFormat(row["Commodity"].ToString(), 15, 2));
                    sw.Write(report.StringFormat(report.DecimalformatForWeight(row["NetWt"].ToString()), 10, 1));
                    sw.WriteLine("");
                    i = CheckRepeatValue == false ? i + 1 : i;
                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            if (!isDataAvailable)
            {
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="list"></param>
        /// <param name="entity"></param>
        public void StockReceiptAbstractStackNoAndCommodity(StreamWriter sw, List<StockReceiptList> list, CommonEntity entity)
        {
            int count = 11;
            var resultSet = from d in list
                            orderby d.Commodity ascending 
                            group d by new { d.Stackno, d.Commodity } into groupedData
                            select new
                            {
                                Netwt_Kgs = groupedData.Sum(s => s.NetWt),
                                No_Bags = groupedData.Sum(s => s.NoPacking),
                                GroupByNames = groupedData.Key
                            };
            AddHeaderforAbstractStackAndCommodity(sw, entity);

            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("--------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractStackAndCommodity(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.Stackno, 14, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 24, 2));
               // sw.Write(report.StringFormat(item.GroupByNames.Packingtype.ToString(), 22, 2));
                sw.Write(report.StringFormat(item.No_Bags.ToString(), 13, 2));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 20, 1));
                count++;
                sw.WriteLine("");
            }
            sw.WriteLine("--------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="list"></param>
        /// <param name="entity"></param>
        public void StockReceiptAbstractSchemeAndCommodityWise(StreamWriter sw, List<StockReceiptList> list, CommonEntity entity)
        {
            int count = 11;
            var resultSet = from d in list
                            orderby d.Scheme ascending
                            group d by new { d.Scheme, d.Commodity } into groupedData
                            select new
                            {
                                Netwt_Kgs = groupedData.Sum(s => s.NetWt),
                                No_Bags = groupedData.Sum(s => s.NoPacking),
                                GroupByNames = groupedData.Key
                            };
            AddHeaderforAbstractSchemeAndCommodityWise(sw, entity);

            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractSchemeAndCommodityWise(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.Scheme, 19, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 22, 2));
                sw.Write(report.StringFormat(item.No_Bags.ToString(), 14, 1));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 20, 1));
                count++;
                sw.WriteLine("");
            }
            sw.WriteLine("------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="list"></param>
        /// <param name="entity"></param>
        public void StockReceiptAbstractRecdTypeAndSchemeWise(StreamWriter sw, List<StockReceiptList> list, CommonEntity entity)
        {
            int count = 11;
            var resultSet = from d in list
                            orderby d.RecdType, d.Scheme ascending
                            group d by new { d.Scheme, d.RecdType, d.From_Whom_Received, d.Commodity, d.Packingtype } into groupedData
                            select new
                            {
                                Netwt_Kgs = groupedData.Sum(s => s.NetWt),
                                No_Bags = groupedData.Sum(s => s.NoPacking),
                                GroupByNames = groupedData.Key
                            };
            AddHeaderforAbstractRecTypeAndSchemeWise(sw, entity);

            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractRecTypeAndSchemeWise(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.RecdType, 16, 2));
                sw.Write(report.StringFormat(item.GroupByNames.From_Whom_Received, 15, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Scheme, 15, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 21, 2));
                sw.Write(report.StringFormat(item.No_Bags.ToString(), 12, 2));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 18, 1));
                sw.Write(report.StringFormat(item.GroupByNames.Packingtype, 21, 2));
                count++;
                sw.WriteLine("");
            }
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="list"></param>
        /// <param name="entity"></param>
        public void StockReceiptAbstractCommodityWise(StreamWriter sw, List<StockReceiptList> list, CommonEntity entity)
        {
            int count = 11;
            var resultSet = from d in list
                            orderby d.Commodity ascending
                            group d by new { d.Commodity,d.Packingtype } into groupedData
                            select new
                            {
                                Netwt_Kgs = groupedData.Sum(s => s.NetWt),
                                No_Bags = groupedData.Sum(s => s.NoPacking),
                                GroupByNames = groupedData.Key
                            };
            AddHeaderforAbstractCommodityWise(sw, entity);

            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("----------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractCommodityWise(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 23, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Packingtype.ToString(), 22, 2));
                sw.Write(report.StringFormat(item.No_Bags.ToString(), 12, 1));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 22, 1));
                count++;
                sw.WriteLine("");
            }
            sw.WriteLine("----------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }
    }

    public class StockReceiptList
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Ackno { get; set; }
        public string TruckMemoNo { get; set; }
        public DateTime Date { get; set; }
        public string Lorryno { get; set; }
        public string RecdType { get; set; }
        public string Packingtype { get; set; }
        public string From_Whom_Received { get; set; }
        public string Stackno { get; set; }
        public string Scheme { get; set; }
        public int NoPacking { get; set; }
        public string Commodity { get; set; }
        public double NetWt { get; set; }
    }

    public class ReportParameter
    {
        public string GCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UserName { get; set; }
        public string RCode { get; set; }
        public int Type { get; set; }
    }
}
