using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class TransactionReceipt
    {
        private string GName { get; set; }
        private string RegionName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();

        /// <summary>
        /// Generate the transaction receipt
        /// </summary>
        /// <param name="entity">Common entity</param>
        public void GenerateTransactionReceipt(CommonEntity entity)
        {
            string fPath = string.Empty, sPath = string.Empty, sFileName = string.Empty;
            string filePath = string.Empty;
            StreamWriter sw = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RegionName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                sFileName = entity.GCode + GlobalVariable.StockTruckMemoRegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                sPath = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(sPath);
                //delete file if exists
                filePath = sPath + "//" + sFileName + ".txt";
                report.DeleteFileIfExists(filePath);
                sw = new StreamWriter(filePath, true);
                WriteTransactionForAbstract(sw, entity);
                sw.Flush();
                sw.Close();
                //send mail to corresponding godown.

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Write the Transaction Abstract data
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void WriteTransactionForAbstract(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 10;
            var distinctCommodity = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Commodity");
            //Date wise DO report
            string sDoNo = string.Empty;
            string sCommodity = string.Empty;
            decimal dTotal = 0;
            AddHeaderForTransactionReceipt(sw, entity);
            foreach (DataRow dateValue in distinctCommodity.Rows)
            {
                iCount = 11;
                bool CheckRepeatValue = false;
                sCommodity = string.Empty;
                sDoNo = string.Empty;
                DataRow[] datas = entity.dataSet.Tables[0].Select("Commodity='" + dateValue["Commodity"] + "'");

                foreach (var item in datas)
                {
                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("-------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeaderForTransactionReceipt(sw, entity);
                    }
                    sCommodity = Convert.ToString(item["Commodity"]);
                    if (sDoNo == sCommodity)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        sDoNo = sCommodity;
                    }

                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? sCommodity : " ", 34, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Date"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Trans_action"].ToString(), 21, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(item["Quantity"].ToString()), 15, 1));
                    sw.WriteLine("");
                    iCount = iCount + 1;
                    dTotal += Convert.ToDecimal(item["Quantity"].ToString());
                }
                sw.WriteLine("-------------------------------------------------------------------------------");
                sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                sw.Write(report.StringFormatWithoutPipe("", 10, 2));
                sw.Write(report.StringFormatWithoutPipe("", 21, 2));
                sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(dTotal.ToString()), 15, 1));
                sw.WriteLine("");
            }
            sw.WriteLine("-------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for Transaction receipt
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderForTransactionReceipt(StreamWriter sw,CommonEntity entity)
        {
            sw.WriteLine("    TAMILNADU CIVIL SUPPLIES CORPORATION      Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("      Receipt Date Wise Abstract Details of " + GName+ " Godown");
            sw.WriteLine(" ");
            sw.WriteLine("From: " + report.FormatDate(entity.FromDate) + " to "+ report.FormatDate(entity.Todate) + "    Weight in Kilo Grams");
            sw.WriteLine("-------------------------------------------------------------------------------");
            sw.WriteLine("Commodity                          Date       Transaction        Net Weight    ");
            sw.WriteLine("-------------------------------------------------------------------------------");

        }

    }
    public class TransactionReceiptEntity
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Commodity { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public string Issuingcode { get; set; }
        public string Transaction { get; set; }
    }
}
