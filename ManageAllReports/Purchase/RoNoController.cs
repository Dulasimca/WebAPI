using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.ManageAllReports.Purchase
{
    public class RoNoController
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
        public void GenerateRoNoPurchase(CommonEntity entity)
        {
            //AuditLog.WriteError("GenerateRoNoPurchaseReport");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                //GName = entity.GName;
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RegionName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.RoNoPurchaseFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                WriteRoNoPurchase(streamWriter, entity);
                List<RoNoPurchaseEntity> RoNoPurchase = new List<RoNoPurchaseEntity>();
                RoNoPurchase = report.ConvertDataTableToList<RoNoPurchaseEntity>(entity.dataSet.Tables[0]);

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
        public void WriteRoNoPurchase(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 10;
            var distinctCoop = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Depositor");
            //var distinctCommodity = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Commodity");
            int i = 1;
            string sAckno = string.Empty;
            string sDepositor = string.Empty;
            string sCommodity = string.Empty;
            decimal dTotal = 0;
            //decimal gTotal = 0;
            AddHeaderForRoNoPurchase(sw, entity);
            foreach (DataRow dateValue in distinctCoop.Rows)
            {
                iCount = 11;
                bool CheckRepeatValue = false;
                sAckno = string.Empty;
                sDepositor = string.Empty;
                DataRow[] datas = entity.dataSet.Tables[0].Select("Depositor='" + dateValue["Depositor"] + "'");

                foreach (var item in datas)
                {
                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeaderForRoNoPurchase(sw, entity);
                    }
                    // var sortedlist = sCommodity.OrderBy(s => s.sComm).ThenBy(s1 => s1.StudentID);
                    //sCommodity = item.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, true));
                    sDepositor = Convert.ToString(item["Depositor"]);
                    if (sAckno == sDepositor)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        sAckno = sDepositor;
                    }
                    sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Ackno"].ToString(), 11, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(item["Date"].ToString()), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? sDepositor : " ", 35, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Commodity"].ToString(), 27, 2));
                    sw.Write(report.StringFormatWithoutPipe(item["Bags"].ToString(), 6, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(item["NetWeight"].ToString()), 14, 1));
                    sw.Write(report.StringFormatWithoutPipe((item["TruckMemoNo"].ToString()), 15, 1));
                    sw.Write(report.StringFormatWithoutPipe((item["OrderNo"].ToString()), 14, 2));
                    sw.Write(report.StringFormatWithoutPipe((item["Lorryno"].ToString()), 13, 2)); 
                    sw.Write(report.StringFormatWithoutPipe((item["Scheme"].ToString()), 15, 2)); 
                    sw.WriteLine("");
                    dTotal += Convert.ToDecimal(item["NetWeight"].ToString());
                    // gTotal += Convert.ToDecimal(item["Quantity"].ToString());
                    i = i + 1;
                    iCount++;
                }
                //sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.Write(" ");
                sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                sw.Write(report.StringFormatWithoutPipe("", 27, 2));
                sw.Write(report.StringFormatWithoutPipe("Total".ToString(), 11, 2));
                sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(dTotal.ToString()), 37, 1));
                dTotal = 0;
                sw.WriteLine("");
                // sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            }
        }
        /// <summary>
        /// Add header for Transaction receipt
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderForRoNoPurchase(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                                               TAMILNADU CIVIL SUPPLIES CORPORATION                                               Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                      RONO Purchase Receipt Details of " + GName + " Godown");
            sw.WriteLine(" ");
            sw.WriteLine(" From: " + report.FormatDate(entity.FromDate) + " to " + report.FormatDate(entity.Todate) + "                                                                 Weight in Kilo Grams                             Page No: 1");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No Ack.No      Date       Place                               Commodity                     Bags     Net Weight   T.Memo No     Order No       Lorry No      Scheme         |");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");

        }

    }

}


public class RoNoPurchaseEntity
{
    public string Region { get; set; }
    public string Godownname { get; set; }
    public string Ackno { get; set; }
    public DateTime Date { get; set; }
    public string Depositor { get; set; }
    public string Lorryno { get; set; }
    public string Orderno { get; set; }
    public double Quantity { get; set; }
    public string Bags { get; set; }
    public string TruckMen { get; set; }
    public string Type { get; set; }

}