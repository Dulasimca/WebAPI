using System;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports
{
    public class WriteOff
    {
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateWriteOffReport(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.WriteOFFReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                streamWriter = new StreamWriter(filePath, true);
                CommodityWiseIssueMemoReport(streamWriter, entity);
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
            sw.WriteLine("      Godown Name: " + entity.GName + "    Stock Issue W/off Register ");
            sw.WriteLine("      Issue Date: " + report.FormatDate(entity.FromDate) + "    To : " + report.FormatDate(entity.Todate) + "  Page No :" + pageNo.ToString());
            sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine(" SNO.  DATE      DOCUMENT NO  COMMODITY        NET.WEIGHT  STACK NO            REMARKS");
            sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void CommodityWiseIssueMemoReport(StreamWriter sw, CommonEntity entity)
        {
            try
            {
                int count = 8;
                var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Commodity");
                int pageNo = 1;
                decimal netweight = 0;
                AddHeader(sw, entity, pageNo);
                int i = 1;
                foreach (DataRow nrow in dateList.Rows)
                {
                    netweight = 0;
                    DataRow[] drdata = entity.dataSet.Tables[0].Select("Commodity='" + Convert.ToString(nrow["Commodity"]) + "'");
                    foreach (DataRow row in drdata)
                    {

                        if (count >= 50)
                        {
                            //Add header again
                            pageNo++;
                            count = 8;
                            sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            sw.WriteLine((char)12);
                            AddHeader(sw, entity, pageNo);
                        }
                        sw.Write("");
                        sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Issue_Date"].ToString(), 10, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["Issueno"].ToString(), 11, 1) + " ");
                        sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 16, 2));
                        sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(row["NetWt"].ToString()), 11, 1) + "  ");
                        sw.Write(report.StringFormatWithoutPipe(row["Stackno"].ToString(), 14, 2));
                        sw.Write(report.StringFormatWithoutPipe(row["remarks"].ToString(), 101, 2));
                        sw.WriteLine("");
                        count = count + 1;
                        netweight += !string.IsNullOrEmpty(row["NetWt"].ToString()) ? Convert.ToDecimal(row["NetWt"]) : 0;
                        i++;
                    }
                    sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    sw.Write("");
                    sw.Write(report.StringFormatWithoutPipe("", 4, 2));
                    sw.Write(report.StringFormatWithoutPipe("Total", 10, 2));
                    sw.Write(report.StringFormatWithoutPipe("-", 11, 1) + " ");
                    sw.Write(report.StringFormatWithoutPipe("-", 16, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(Convert.ToString(netweight)), 11, 1) + "  ");
                    sw.Write(report.StringFormatWithoutPipe("-", 14, 2));
                    sw.Write(report.StringFormatWithoutPipe("-", 101, 2));
                    sw.WriteLine("");
                    sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    count = count + 1;
                }
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
