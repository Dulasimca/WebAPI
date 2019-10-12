using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports
{
    public class HullingDetails
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
        public void GenerateHullingReport(CommonEntity entity)
        {
            //AuditLog.WriteError("GenerateHullingReport");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.HullingDetailsReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseHullingDetails(streamWriter, entity);
                List<HullingReportEntity> hullingReportList = new List<HullingReportEntity>();
                hullingReportList = report.ConvertDataTableToList<HullingReportEntity>(entity.dataSet.Tables[0]);
                HullingDetailsAbstract(streamWriter, hullingReportList, entity);

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
            sw.WriteLine("                         TAMILNADU CIVIL SUPPLIES CORPORATION      Region: " + RName + " Report Date : " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Hulling Rice Receipt Details of " + GName + " Godown ");
            sw.WriteLine(" ");
            sw.WriteLine("          From:" + report.FormatDate(entity.FromDate) + "   To:" + report.FormatDate(entity.Todate) + "               Weight in Kilo Grams                 Page No: 1");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|      Ack No      |      Date      |            Hulling Name             |        Commodity         |    Bags    |     Net Weight    |");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
            // sw.WriteLine("    |                  |          |                                     |                          |            |                   |");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void AddHeaderforAbstractDetails(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("    Hulling Rice Receipt Abstract Details of Godown: " + GName + "  Report Date : " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            //  sw.WriteLine("          From:" + report.FormatDate(date) + "   To:" + report.FormatDate(date) + " Weight in Kilo Grams       Page No: 1");
            sw.WriteLine("       Abstract:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "       Region :" + RName);
            sw.WriteLine("---------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("            Hulling Name             |        Commodity         |    Bags    |    Net Wt (in Kgs)/Nos    |");
            sw.WriteLine("---------------------------------------------------------------------------------------------------------|");
        }
        ///</summary>
        ///
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseHullingDetails(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 11;
            int i = 1;
            string SRNO = string.Empty;
            bool isDataAvailable = false;
            // foreach (DataRow dateValue in distinctDate.Rows)
            //{
            isDataAvailable = true;
            AddHeader(sw, entity);
            // DataRow[] datas = entity.dataSet.Tables[0].Select("HullingReportDate='" + dateValue["HullingReportDate"] + "'");
            foreach (DataRow dr in entity.dataSet.Tables[0].Rows)
            {
                if (iCount >= 50)
                {
                    iCount = 11;
                    sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeader(sw, entity);
                }
                //sIssuer = Convert.ToString(dr["DepositorName"]).Trim();
                sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                sw.Write(report.StringFormatWithoutPipe(dr["SRNo"].ToString(), 18, 2));
                sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(dr["SRDate"].ToString()), 16, 2));
                sw.Write(report.StringFormatWithoutPipe(Convert.ToString(dr["DepositorName"]).Trim(), 37, 2));
                sw.Write(report.StringFormatWithoutPipe(dr["ITDescription"].ToString(), 26, 2));
                sw.Write(report.StringFormatWithoutPipe(dr["NoPacking"].ToString(), 12, 1));
                sw.Write(report.StringFormatWithoutPipe(dr["Nkgs"].ToString(), 19, 1));
                sw.WriteLine("");
                sw.WriteLine(" ");
                i = i + 1;
                iCount++;
            }
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
            if (!isDataAvailable)
                {
                    sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                }
            }

        /// <summary>
        /// Write text file for item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="hullingReportList">DOReg entity</param>
        /// <param name="entity">Common Entity</param>
        public void HullingDetailsAbstract(StreamWriter sw, List<HullingReportEntity> hullingReportList, CommonEntity entity)
        {
            try
            {
                int iCount = 11;
                var myResultItem = from a in hullingReportList
                                   group a by new { a.ITDescription, a.DepositorName } into gValue
                                   select new
                                   {
                                       Nkgs = gValue.Sum(s => s.Nkgs),
                                       NoPacking = gValue.Sum(s => s.NoPacking),
                                       GroupByNames = gValue.Key
                                   };
                double dAmount = 0;
                AddHeaderforAbstractDetails(sw, entity);
                foreach (var item in myResultItem)
                {
                    if (iCount >= 50)
                    {
                        iCount = 11;
                        sw.WriteLine("----------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeaderforAbstractDetails(sw, entity);
                    }
                    sw.Write(report.StringFormat(item.GroupByNames.DepositorName, 37, 2));
                    sw.Write(report.StringFormat(item.GroupByNames.ITDescription, 26, 2));
                    sw.Write(report.StringFormat(report.Decimalformat(item.NoPacking.ToString()), 12, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(item.Nkgs.ToString()), 27, 1));
                   // sw.Write(report.StringFormat(report.Decimalformat(item.Itemamount.ToString()), 20, 1));
                    dAmount = dAmount + item.Nkgs;
                    iCount++;
                    sw.WriteLine("");
                    sw.WriteLine("");
                }
                // Add toal values
                sw.WriteLine("---------------------------------------------------------------------------------------------------------|");
                sw.Write(report.StringFormat("Total Amount", 64, 1));
                sw.Write(report.StringFormat(report.Decimalformat(dAmount.ToString()), 40, 1));
                sw.WriteLine("");
                sw.WriteLine("---------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
            }

        }


    }


    public class HullingReportEntity
    {
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string SRNo { get; set; }
        public DateTime SRDate { get; set; }
        public string ITDescription { get; set; }
        public string DepositorName { get; set; }
        public double NoPacking { get; set; }
        public double Nkgs { get; set; }
    }
}
