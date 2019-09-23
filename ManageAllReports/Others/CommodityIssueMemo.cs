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
            AuditLog.WriteError("GenerateCommodityIssueMemoReport");
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
            int count = 10;
            var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
            int i = 1;
            string SINo = string.Empty;
            string issuedTo = string.Empty;
            bool CheckRepeatValue = false;
            bool isDataAvailable = false;
            foreach (DataRow date in dateList.Rows)
            {
                isDataAvailable = true;
                count = 11;
                string SINoNext = string.Empty;
                AddHeader(sw, entity);
                DataRow[] data = entity.dataSet.Tables[0].Select("Date='" + date["Issue_Date"] + "'");
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
                    SINoNext = row["Issue_Memono"].ToString();
                    issuedTo = Convert.ToString(row["RecdFrom"]).Trim();
                    if (SINo == SINoNext)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        SINo = SINoNext;
                    }
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? i.ToString() : " ", 4, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? SINoNext : " ", 16, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Issue_Date"].ToString() : " ", 12, 1));
                    sw.Write(report.StringFormat(row["Commodity"].ToString(), 17, 2));
                    sw.Write(report.StringFormat(row["Quantity"].ToString(), 18, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? issuedTo : " ", 24, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? row["Lorryno"].ToString() : " ", 11, 1));
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