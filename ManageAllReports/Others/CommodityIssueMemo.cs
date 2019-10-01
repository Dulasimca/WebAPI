using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports
{
    public class CommodityIssueMemo
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
        public void GenerateCommodityIssueMemoReport(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.CommodityIssueMemoReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseCommodityIssueMemoReport(streamWriter, entity);

                List<CommodityIssueMemoEntity> commodityIssueList = new List<CommodityIssueMemoEntity>();
                commodityIssueList = report.ConvertDataTableToList<CommodityIssueMemoEntity>(entity.dataSet.Tables[0]);
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
            sw.WriteLine("                                        Issue Memo Datewise Details of - Commodity    Godown : " + GName);
            sw.WriteLine(" ");
            sw.WriteLine("          From:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|  I.MEMO NO     |Date        |Commodity        |NET Weight(Kgs)   |ISSUED TO               |Lorry No   |");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |                |            |                 |                  |                        |           |");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseCommodityIssueMemoReport(StreamWriter sw, CommonEntity entity)
        {
            try
            {
                int count = 10;
                // var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
                int i = 1;
                string issuedTo = string.Empty;
                AddHeader(sw, entity);
                foreach (DataRow row in entity.dataSet.Tables[0].Rows)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 11;
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeader(sw, entity);
                    }
                    issuedTo = Convert.ToString(row["Issuedto"]).Trim();
                    sw.Write(report.StringFormat(i.ToString(), 4, 2));
                    sw.Write(report.StringFormat(row["Issue_Memono"].ToString(), 16, 1));
                    sw.Write(report.StringFormat(row["Issue_Date"].ToString(), 12, 1));
                    sw.Write(report.StringFormat(row["Commodity"].ToString(), 17, 2));
                    sw.Write(report.StringFormat(row["Quantity"].ToString(), 18, 1));
                    sw.Write(report.StringFormat(issuedTo, 24, 2));
                    sw.Write(report.StringFormat(row["Lorryno"].ToString(), 11, 1));
                    sw.WriteLine("");
                    sw.WriteLine(" ");
                    i = i + 1;
                    count = count + 2;
                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            catch (Exception ex )
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
public class CommodityIssueMemoEntity
{
    public string Region { get; set; }
    public string Godownname { get; set; }
    public string Scheme { get; set; }
    public DateTime Issue_Date { get; set; }
    public string Commodity { get; set; }
    public string Issue_Memono { get; set; }
    public double Quantity { get; set; }
    public string Lorryno { get; set; }
    public string Issuedto { get; set; }
}