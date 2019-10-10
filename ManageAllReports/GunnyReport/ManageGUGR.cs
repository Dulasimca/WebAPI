using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models;

namespace TNCSCAPI.ManageAllReports.GunnyReport
{
    public class ManageGUGR
    {
        private string GName { get; set; }
        private string RName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        private string HeaderTitle { get; set; }
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateGUGRReport(CommonEntity entity, GUGRParameter param)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                HeaderTitle = (param.Type == "GR") ? "      Gunny Release Details of " : "      Gunny Utilisation Details of ";
                fileName = (param.Type == "GR") ? (entity.GCode + GlobalVariable.GRReportFileName) : (entity.GCode + GlobalVariable.GUReportFileName);
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseCommodityWiseGUGRReport(streamWriter, entity);

                List<GUList> guList = new List<GUList>();
                guList = report.ConvertDataTableToList<GUList>(entity.dataSet.Tables[0]);
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
        public void AddHeader(StreamWriter sw, CommonEntity entity, int pageNo)
        {
            sw.WriteLine("      TamilNadu Civil Supplies Corporation       " + entity.RName);
            sw.WriteLine(HeaderTitle + entity.GName + " Godown");
            sw.WriteLine("      From : " + report.FormatDate(entity.FromDate) + "    To : " + report.FormatDate(entity.Todate) + "  Page No :" + pageNo.ToString());
            sw.WriteLine("--------------------------------------------------------------------------------------------------------");
            sw.WriteLine("Ack.No     Date      Commodity       Bundles  Nos       STACK NO    SYEAR Stack Commodity");
            sw.WriteLine("--------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseCommodityWiseGUGRReport(StreamWriter sw, CommonEntity entity)
        {
            try
            {
                int count = 8;
                var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Date");
                int pageNo = 1;
                decimal qty = 0;
                int Bags = 0;
                AddHeader(sw, entity, pageNo);
                int i = 1;
                foreach (DataRow nrow in dateList.Rows)
                {
                    DataRow[] drdata = entity.dataSet.Tables[0].Select("Date='" + Convert.ToString(nrow["Date"]) + "'");
                    foreach (DataRow row in drdata)
                    {

                        if (count >= 50)
                        {
                            //Add header again
                            pageNo++;
                            count = 8;
                            sw.WriteLine("--------------------------------------------------------------------------------------------------------");
                            sw.WriteLine((char)12);
                            AddHeader(sw, entity, pageNo);
                        }
                        sw.Write("");
                        sw.Write(report.StringFormatWithoutPipe(row["Ackno"].ToString(), 10, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Date"].ToString(), 9, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 16, 2));
                        sw.Write(report.StringFormatWithoutPipe("0", 8,2));
                        sw.Write(report.StringFormatWithoutPipe(row["Bags"].ToString(), 9, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["stackno"].ToString(), 10, 2));
                    //  sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(row["Quantity"].ToString()), 11, 1) + "  ");
                        sw.Write(report.StringFormatWithoutPipe(row["Year"].ToString(), 5, 1));
                        sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 16, 2));
                        sw.WriteLine("");
                        count = count + 1;
                      //  qty += !string.IsNullOrEmpty(row["Quantity"].ToString()) ? Convert.ToDecimal(row["Quantity"]) : 0;
                        Bags += !string.IsNullOrEmpty(Convert.ToString(row["Bags"])) ? Convert.ToInt32(row["Bags"].ToString()) : 0;
                        i++;
                    }
                  
                  //  count = count + 1;
                }
                sw.WriteLine("--------------------------------------------------------------------------------------------------------");
                sw.Write("");
                sw.Write(report.StringFormatWithoutPipe("", 10, 2));
                sw.Write(report.StringFormatWithoutPipe("Total", 9, 2));
                sw.Write(report.StringFormatWithoutPipe("-", 16, 2));
                sw.Write(report.StringFormatWithoutPipe("-", 8, 2));
                sw.Write(report.StringFormatWithoutPipe((Convert.ToString(Bags)), 9, 2));
                // sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(Convert.ToString(qty)), 11, 1) + "  ");
                sw.Write(report.StringFormatWithoutPipe("-", 14, 2));
                sw.Write(report.StringFormatWithoutPipe("-", 101, 2));
                sw.WriteLine("");
                sw.WriteLine("--------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
        }
    }

    public class GUList
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Ackno { get; set; }
        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public int Bags { get; set; }
        public string StackNo { get; set; }
        public double Quantity { get; set; }
        public string Year { get; set; }
    }
}
