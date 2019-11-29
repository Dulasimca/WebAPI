using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.DeliveryOrder
{
    public class ManageDOMargin
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDOMarginReport(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.DOMarginReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseCommodityIssueMemoReport(streamWriter, entity);

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
            sw.WriteLine("  TamilNadu Civil Supplies Corporation          Region Name : " + entity.RName);
            sw.WriteLine("  Godown Name : " + entity.GName + "    Delivery Order Margin Amount Details");
            sw.WriteLine("  D.Ord.Date :" + report.FormatDate(entity.FromDate) + "  To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine(" Godown Name                 SOCIETY  NAME                      DONO AND DATE     COMMODITY                 SCHEME         QUANTITY       RATE   MARGIN AMT.");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
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
                int count = 8;
                int i = 1;
                AddHeader(sw, entity);
                decimal TotalAmount = 0;
                foreach (DataRow row in entity.dataSet.Tables[0].Rows)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 8;
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeader(sw, entity);
                    }
                    sw.Write(report.StringFormatWithoutPipe(row["GodownName"].ToString(), 25, 2));

                    sw.Write(report.StringFormatWithoutPipe(row["Coop"].ToString(), 33, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Dono"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(row["Dodate"].ToString()), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Comodity"].ToString(), 26, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Scheme"].ToString(), 12, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(row["Quantity"].ToString()), 12, 1) +"    ");
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Rate"].ToString()), 6, 1)+" ");
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Amount"].ToString()), 10, 1));
                    sw.WriteLine("");
                    TotalAmount += Convert.ToDecimal(report.Decimalformat(row["Amount"].ToString()));
                    i = i + 1;
                    count = count + 1;
                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.Write(report.StringFormatWithoutPipe(" ", 33, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 35, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 10, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 26, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 12, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 12, 1) + "    ");
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(TotalAmount.ToString()), 18, 1));
                sw.WriteLine("");
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
