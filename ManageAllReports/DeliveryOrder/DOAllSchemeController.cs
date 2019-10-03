using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TNCSCAPI.ManageAllReports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOAllSchemeController : ControllerBase
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
        public void GenerateDOAllSchemeReport(CommonEntity entity)
        {

            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Regionname"].ToString();
                fileName = entity.GCode + GlobalVariable.DOAllSchemeReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                SocietyWiseDOAllSchemeReport(streamWriter, entity);

                List<DOAllSchemeList> dOAllSchemeList = new List<DOAllSchemeList>();
                dOAllSchemeList = report.ConvertDataTableToList<DOAllSchemeList>(entity.dataSet.Tables[0]);
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
            sw.WriteLine("                              TAMILNADU CIVIL SUPPLIES CORPORATION                       " + RName);
            sw.WriteLine(" ");
            sw.WriteLine("              Godown : " + GName +  "Delivery Order Details Society Wise with Issue Details"    );
            sw.WriteLine(" ");
            sw.WriteLine("          D.Ord Date:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate) + "          -EXCESS ");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No DO.No.   DO.Date    Commodity    Scheme     NetWt(Kgs)/NO's Rate       C.Amount     NC.Amount    Amount       |");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------|");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void SocietyWiseDOAllSchemeReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 10;
            var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Coop");
            int i = 1;
            string doNo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool isDataAvailable = false;
            decimal Qty = 0;
            float Rate = 0;
            float C_Amount = 0;
            float NC_Amount = 0;
            float Amount = 0;
            count = 11;
            foreach (DataRow date in dateList.Rows)
            {
                isDataAvailable = true;

                string doNoNext = string.Empty;
                AddHeader(sw, entity);
                DataRow[] data = entity.dataSet.Tables[0].Select("Coop='" + date["Coop"] + "'");
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
                    doNoNext = row["Dono"].ToString();
                   // fromWhomRcd = Convert.ToString(row["RecdFrom"]).Trim();

                    sw.Write(report.StringFormat(i.ToString(), 4, 2));
                    sw.Write(report.StringFormat(doNoNext, 9, 1));
                    sw.Write(report.StringFormat(row["Dodate"].ToString(), 11, 1));
                    sw.Write(report.StringFormat(row["Commodity"].ToString(), 13, 2));
                    sw.Write(report.StringFormat(row["Scheme"].ToString(), 11, 1));
                    sw.Write(report.StringFormat(row["Quantity"].ToString(), 16, 1));
                  // sw.Write(report.StringFormat(fromWhomRcd, 21, 2));
                    sw.Write(report.StringFormat(row["Rate"].ToString(), 11, 1));
                    // sw.Write(report.StringFormat(row["TruckMemoNo"].ToString(), 13, 1));
                    //  sw.Write(report.StringFormat(row["Truckmemodate"].ToString(), 13, 1));
                    sw.Write(report.StringFormat(row["Amount"].ToString(), 13, 1));
                    sw.WriteLine("");
                    Rate += !string.IsNullOrEmpty(Convert.ToString(row["Rate"])) ? Convert.ToInt32(row["Rate"].ToString()) : 0;
              //      C_Amount += !string.IsNullOrEmpty(Convert.ToString(row["Rate"])) ? Convert.ToInt32(row["Rate"].ToString()) : 0;
               //     NC_Amount += !string.IsNullOrEmpty(Convert.ToString(row["Rate"])) ? Convert.ToInt32(row["Rate"].ToString()) : 0;
                    Amount += !string.IsNullOrEmpty(Convert.ToString(row["Amount"])) ? Convert.ToInt32(row["Amount"].ToString()) : 0;
                    Qty += !string.IsNullOrEmpty(Convert.ToString(row["Quantity"])) ? Convert.ToDecimal(row["Quantity"].ToString()) : 0;
                    i = i + 1;
                    count++;
                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.Write(report.StringFormat("", 4, 2));
                sw.Write(report.StringFormat("", 9, 2));
                sw.Write(report.StringFormat("", 11, 2));
                sw.Write(report.StringFormat("", 13, 2));
                sw.Write(report.StringFormat("  Total ", 11, 1));
                sw.Write(report.StringFormatWithoutPipe(Qty.ToString(), 17, 1));
                sw.Write(report.StringFormatWithoutPipe(Rate.ToString(), 8, 1));
                sw.Write(report.StringFormatWithoutPipe(C_Amount.ToString(), 8, 1));
                sw.Write(report.StringFormatWithoutPipe(NC_Amount.ToString(), 17, 1));
                sw.Write(report.StringFormatWithoutPipe(Amount.ToString(), 17, 1));
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                //Total 

                sw.WriteLine((char)12);
            }
            if (!isDataAvailable)
            {
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }

        }

    }

    public class DOAllSchemeList
    {
        public string Regionname { get; set; }
        public string Godownname { get; set; }
        public string Dono { get; set; }
        public DateTime Dodate { get; set; }
        public string Commodity { get; set; }
        public string Scheme { get; set; }
        public string Coop { get; set; }
        public double Quantity { get; set; }
        public float Rate { get; set; }
        public double Amount { get; set; }
        public string C_Nc { get; set; }
        public string Tyname { get; set; }
    }
}