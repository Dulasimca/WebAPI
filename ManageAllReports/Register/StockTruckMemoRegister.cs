using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace TNCSCAPI.ManageAllReports
{
    public class StockTruckMemoRegister
    {
        private string GName { get; set; }
        private string Regioncode { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();
        public void GenerateTruckMemoForRegister(CommonEntity entity)
        {
            string fPath = string.Empty, sPath = string.Empty, sFileName = string.Empty;
            string filePath = string.Empty;
            StreamWriter sw = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                Regioncode = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                sFileName = entity.GCode + GlobalVariable.StockTruckMemoRegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                sPath = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(sPath);
                //delete file if exists
                filePath = sPath + "//" + sFileName + ".txt";
                report.DeleteFileIfExists(filePath);
                sw = new StreamWriter(filePath, true);
                WriteTruckMemoForDateWise(sw, entity);
                // sw.WriteLine((char)12);             
                TruckMemoRegAbstract(sw, entity);
                sw.Flush();
               
                //send mail to corresponding godown.

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                sw.Close();
                sw = null;
                fPath = string.Empty; sFileName = string.Empty;
            }
        }
        public void WriteTruckMemoForDateWise(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 10;
            var distinctDate = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
            //Date wise DO report
            int i = 1;
            string sIssuer = string.Empty;
            string sDoNo = string.Empty;
            foreach (DataRow dateValue in distinctDate.Rows)
            {
                iCount = 11;
                bool CheckRepeatValue = false;
                string sDoNo1 = string.Empty;
                DataRow[] datas = entity.dataSet.Tables[0].Select("Issue_Date='" + dateValue["Issue_Date"] + "'");
                AddHeader(sw, Convert.ToString(dateValue["Issue_Date"]));
                foreach (DataRow dr in datas)
                {

                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeader(sw, Convert.ToString(dateValue["Issue_Date"]));
                    }
                    sDoNo1 = dr["Truck_Memono"].ToString();
                    if (sDoNo == sDoNo1)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        sDoNo = sDoNo1;
                    }

                    sIssuer = Convert.ToString(dr["To_Whom_Issued"]).Trim();
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue==false? i.ToString():" ", 4, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? dr["Truck_Memono"].ToString() : " ", 12, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? dr["Mono"].ToString():" ", 15, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? dr["RoNo"].ToString():" ", 11, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ?  sIssuer:" ", 31, 2));
                    sw.Write(report.StringFormatWithoutPipe(dr["Scheme"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(dr["Stackno"].ToString(), 8, 2));
                    sw.Write(report.StringFormatWithoutPipe(dr["NoBags"].ToString(), 7, 1));
                    sw.Write(report.StringFormatWithoutPipe(dr["Commodity"].ToString(), 25, 2));
                    sw.Write(report.StringFormatWithoutPipe(dr["NetWt"].ToString(), 7, 2));
                    sw.WriteLine("");
                    iCount = iCount + 1;
                    i = CheckRepeatValue == false ? i + 1 : i;
                }
                sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
        }
        public void AddHeader(StreamWriter sw, string Date)
        {
            sw.WriteLine("                                  TAMILNADU CIVIL SUPPLIES CORPORATION                  Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Truck Memo Register");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(Date) + " (Net Wt in kgs/Klts/Nos)   Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("S.No Truck Memo   Mo.No           Ro.No       To Whom Issued                  Scheme     StackNo  No bags Commodity                 Net wt  ");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------");
        }
        public void AddHeaderforTMAbstract(StreamWriter sw, string Date)
        {
            sw.WriteLine("                                  TAMILNADU CIVIL SUPPLIES CORPORATION                  Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Truck Memo Register Abstract ");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(Date) + " (Net Wt in kgs/Klts/Nos)    Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("StackNo       Commodity                                   No bags         Net Wt(in Kgs)/Nos  ");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
        }

        public void TruckMemoRegAbstract(StreamWriter sw, CommonEntity entity)
        {
            var distinctDate = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
            //Date wise DO report
            int iCount = 11;
            string sIssuer = string.Empty;
            string sDoNo = string.Empty;
            foreach (DataRow dateValue in distinctDate.Rows)
            {
                iCount = 11;
                string sDoNo1 = string.Empty;
                DataRow[] datas = entity.dataSet.Tables[0].Select("Issue_Date='" + dateValue["Issue_Date"] + "'");
                List<TruckMemoRegEntity> dORegEntities = new List<TruckMemoRegEntity>();
                dORegEntities = report.ConvertDataRowToList<TruckMemoRegEntity>(datas);

                // Gets the group by values based on ths column To_Whom_Issued, Commodity,Scheme
                var myResult = from a in dORegEntities
                               group a by new { a.Stackno, a.Commodity } into gValue
                               select new
                               {
                                   NoBags = gValue.Sum(s => s.NoBags),
                                   NetWt = gValue.Sum(s => s.NetWt),
                                   GroupByNames = gValue.Key
                               };
                AddHeaderforTMAbstract(sw, dateValue["Issue_Date"].ToString());
                foreach (var item in myResult)
                {
                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeaderforTMAbstract(sw, dateValue["Issue_Date"].ToString());
                    }
                    sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Stackno, 14, 2));
                    sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 44, 2));
                    sw.Write(report.StringFormatWithoutPipe(item.NoBags.ToString(), 10, 1));
                    sw.Write(report.StringFormatWithoutPipe(item.NetWt.ToString(), 25, 1));
                    iCount++;
                    sw.WriteLine("");
                }
                sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
        }
    }
    public class TruckMemoRegEntity
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Truck_Memono { get; set; }
        public string Mono { get; set; }
        public DateTime Issue_Date { get; set; }
        public string RoNo { get; set; }
        public string To_Whom_Issued { get; set; }
        public string Stackno { get; set; }
        public string Scheme { get; set; }
        public int? NoBags { get; set; }
        public string Commodity { get; set; }
        public double NetWt { get; set; }

    }
}
