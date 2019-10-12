using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.Sales
{
    public class SalesIssueMemo
    {
        private string GName { get; set; }
        private string RegionName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();

        /// <summary>
        /// Generate the Customer detail
        /// </summary>
        /// <param name="entity">Common entity</param>
        public void GenerateCustomerDetail(CommonEntity entity)
        {
            //AuditLog.WriteError("GenerateHullingReport");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RegionName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.SalesIssueMemoFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                WriteSalesIssueMemo(streamWriter, entity);
                List<IssueMemoCustomerDetailEntity> hullingReportList = new List<IssueMemoCustomerDetailEntity>();
                hullingReportList = report.ConvertDataTableToList<IssueMemoCustomerDetailEntity>(entity.dataSet.Tables[0]);
                //HullingDetailsAbstract(streamWriter, hullingReportList, entity);

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
        /// Write the Sales Customer Details
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void WriteSalesIssueMemo(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 10;
            var distinctCoop = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Coop");
            //Date wise DO report
            string sAckno = string.Empty;
            string sCoop = string.Empty;
            string sCommodity = string.Empty;
            decimal dTotal = 0;
            decimal gTotal = 0;
            AddHeaderForCustomerDetail(sw, entity);
            foreach (DataRow dateValue in distinctCoop.Rows)
            {
                iCount = 11;
                int i = 1;
                bool CheckRepeatValue = false;
                sCoop = string.Empty;
                sAckno = string.Empty;
                sCommodity = string.Empty;
                DataRow[] datas = entity.dataSet.Tables[0].Select("Coop='" + dateValue["Coop"] + "'");
                //DataRow[] datac = entity.dataSet.Tables[0].Select("Commodity='" + dateValue["Commodity"] + "'");

                foreach (var item in datas)
                {
                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeaderForCustomerDetail(sw, entity);
                    }
                    sCoop = Convert.ToString(item["Coop"]);
                    if (sAckno == sCoop)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        sAckno = sCoop;
                    }
                    //sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Ackno"].ToString(), 12, 2));
                    //sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(dateValue["Date"].ToString()), 16, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(item["Date"].ToString()), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? sCoop : " ", 35, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Scheme"].ToString(), 13, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Commodity"].ToString(), 18, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(item["Quantity"].ToString()), 11, 1));
                    sw.Write(report.StringFormatWithoutPipe((item["Rate"].ToString()), 10, 1));
                    sw.Write(report.StringFormatWithoutPipe((item["Value"].ToString()), 11, 1));
                    sw.WriteLine("");
                    iCount = iCount + 1;
                    dTotal += Convert.ToDecimal(item["Quantity"].ToString());
                    gTotal += Convert.ToDecimal(item["Quantity"].ToString());
                    i = i + 1;
                }
                    //sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
                    sw.Write(" ");
                    sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 23, 2));
                    sw.Write(report.StringFormatWithoutPipe("Total".ToString(), 11, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(dTotal.ToString()), 32, 1));
                    dTotal = 0;
                    sw.WriteLine("");
                    //sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
            }

            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
            sw.Write(" ");
            sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
            sw.Write(report.StringFormatWithoutPipe("", 23, 2));
            sw.Write(report.StringFormatWithoutPipe("Grand Total".ToString(), 11, 2));
            sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(gTotal.ToString()), 32, 1));
            sw.WriteLine("");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for Transaction receipt
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderForCustomerDetail(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                             TAMILNADU CIVIL SUPPLIES CORPORATION                      Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                        Stock Issue Register (Society/CRS/NMP) of  " + GName + " Godown");
            sw.WriteLine(" ");
            sw.WriteLine(" From: " + report.FormatDate(entity.FromDate) + " to " + report.FormatDate(entity.Todate) + "                                                              Page No: 1");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("Issue No.    Issue Date    To whom                          Scheme        Commodity          NET. WEIGHT       Rate     Value   |");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");

        }

    }

    public class IssueMemoCustomerDetailEntity
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Ackno { get; set; }
        public DateTime Date { get; set; }
        public string Coop { get; set; }
        public string Scheme { get; set; }
        public string Commodity { get; set; }
        public double Quantity { get; set; }
        public string Bags { get; set; }
        public string Rate { get; set; }
        public string Value { get; set; }
    }

}

