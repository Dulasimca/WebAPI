using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports.Sales
{
    public class SalesIssueMemoAbstract
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
        public void GenerateSalesIssueMemoAbstract(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.GName;
                RName = entity.RName;
                //GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                //RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.SalesIssueMemoAbstractFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                WriteSalesIssueMemoAbstract(streamWriter, entity);

                //List<CommodityIssueMemoEntity> commodityIssueList = new List<CommodityIssueMemoEntity>();
                //commodityIssueList = report.ConvertDataTableToList<CommodityIssueMemoEntity>(entity.dataSet.Tables[0]);
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
            sw.WriteLine("                          TAMILNADU CIVIL SUPPLIES CORPORATION           Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                 Stock Issue Register Abstract (Society/CRS/NMP) of  " + GName + " Godown");
            sw.WriteLine(" ");
            sw.WriteLine(" From: " + report.FormatDate(entity.FromDate) + " to " + report.FormatDate(entity.Todate) + "                                                    Page No: 1");
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No       To whom                  Scheme           Commodity                    NET. WEIGHT       Rate      Value  |");
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void WriteSalesIssueMemoAbstract(StreamWriter sw, CommonEntity entity)
        {
            try
            {
                int count = 10;
                var distinctCommodity = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Commodity");
                // var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
                int i = 1;
                string society = string.Empty;
                string sCommodity = string.Empty;
                decimal dTotal = 0;
                decimal gTotal = 0;
                AddHeader(sw, entity);
                foreach (DataRow dateValue in distinctCommodity.Rows)
                {
                    sCommodity = string.Empty;
                    DataRow[] datas = entity.dataSet.Tables[0].Select("Commodity='" + dateValue["Commodity"] + "'");
                    //DataRow[] datac = entity.dataSet.Tables[0].Select("Commodity='" + dateValue["Commodity"] + "'");

                    foreach (var row in datas)
                    {
                        if (count >= 50)
                        {
                            //Add header again
                            count = 11;
                            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------|");
                            sw.WriteLine((char)12);
                            AddHeader(sw, entity);
                        }
                        society = Convert.ToString(row["Society"]).Trim();
                        sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                        sw.Write(report.StringFormatWithoutPipe(society, 30, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Scheme"].ToString(), 16, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 27, 2) + "  ");
                        sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight  (row["Quantity"].ToString()), 10, 1));
                        sw.Write(report.StringFormatWithoutPipe(row["Rate"].ToString(), 10, 1));
                        sw.Write(report.StringFormatWithoutPipe(row["Value"].ToString(), 10, 1));
                        //sw.WriteLine("");
                        sw.WriteLine(" ");
                        dTotal += Convert.ToDecimal(row["Quantity"].ToString());
                        gTotal += Convert.ToDecimal(row["Quantity"].ToString());
                        i = i + 1;
                        count = count + 2;
                    }
                    //sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
                    sw.Write(" ");
                    sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                    sw.Write(report.StringFormatWithoutPipe("Total".ToString(), 11, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 16, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(dTotal.ToString()), 28, 1));
                    sw.WriteLine(" ");
                    //sw.WriteLine("");
                    dTotal = 0;
                    //sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------|");
                }
                sw.WriteLine("---------------------------------------------------------------------------------------------------------------------|");
                sw.Write(" ");
                sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                sw.Write(report.StringFormatWithoutPipe("Grand Total".ToString(), 11, 2));
                sw.Write(report.StringFormatWithoutPipe("", 16, 2));
                sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(gTotal.ToString()), 28, 1));
                sw.WriteLine("");
                sw.WriteLine("---------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
public class SalesIssueMemoAbstractEntity
{
    public string Region { get; set; }
    public string Godownname { get; set; }
    public string Scheme { get; set; }
    public string Commodity { get; set; }
    public string Society { get; set; }
    public double Quantity { get; set; }
    public string Rate { get; set; }
    public string Value { get; set; }
}