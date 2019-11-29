using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.DeliveryOrder
{
    public class ManageDemandDraft
    {
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDemandDraftReport(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.DODemandDraftBankFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                streamWriter = new StreamWriter(filePath, true);
                DataTable dt = new DataTable();
                dt = entity.dataSet.Tables[0];
                DateWiseCommodityIssueMemoReport(streamWriter, entity, dt);
                streamWriter.Flush();

                // Generate date for 
                streamWriter = null;
                fileName = entity.GCode + GlobalVariable.DODemandDraftDateFileName;
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                streamWriter = new StreamWriter(filePath, true);
                DataTable ndt1 = new DataTable();
                DataView dv = entity.dataSet.Tables[0].DefaultView;
                dv.Sort = "Dodate asc";
                ndt1 = dv.ToTable();
                DateWiseCommodityIssueMemoReport(streamWriter, entity, ndt1);
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
        public void AddHeader(StreamWriter sw, CommonEntity entity,int pageNo)
        {
            sw.WriteLine("      TamilNadu Civil Supplies Corporation       " + entity.RName);
            sw.WriteLine("      Godown Name: " + entity.GName + "    Delivery Order Payment Details ");
            sw.WriteLine("      D.Ord.Date:" + report.FormatDate(entity.FromDate) + "    To : " + report.FormatDate(entity.Todate) +"  Page No :" + pageNo.ToString());
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("   Godown Name           SOCIETY                             DONO       DO DATE      DD.NO      DD DATE       AMOUNT         BANK            CEREAL     NON CEREAL");
            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void DateWiseCommodityIssueMemoReport(StreamWriter sw, CommonEntity entity,DataTable ndt)
        {
            try
            {
                int count = 8;
                // var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
                int pageNo = 1;
                decimal Payment = 0;
                decimal Ceral = 0;
                decimal NonCeral = 0;
                AddHeader(sw, entity, pageNo);
                foreach (DataRow row in ndt.Rows)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        pageNo++;
                        count = 8;
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeader(sw, entity, pageNo);
                    }
                    sw.Write(" ");
                    sw.Write(report.StringFormatWithoutPipe(row["GodownName"].ToString(), 24, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Society"].ToString(), 34, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Dono"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Dodate"].ToString(), 10, 2) + "  ");
                    sw.Write(report.StringFormatWithoutPipe(row["Chequeno"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Chequedate"].ToString(), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["PaymentAmount"].ToString()), 12, 1)+"  ");
                    sw.Write(report.StringFormatWithoutPipe(row["Bank"].ToString(), 14, 2));                   
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Cereal"].ToString()), 12, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["NonCereal"].ToString()), 12, 1));
                    sw.WriteLine("");
                    count = count + 1;
                    Payment += !string.IsNullOrEmpty(row["PaymentAmount"].ToString()) ? Convert.ToDecimal(row["PaymentAmount"]) : 0;
                    Ceral += !string.IsNullOrEmpty(row["Cereal"].ToString()) ? Convert.ToDecimal(row["Cereal"]) : 0;
                    NonCeral += !string.IsNullOrEmpty(row["NonCereal"].ToString()) ? Convert.ToDecimal(row["NonCereal"]) : 0;

                }
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.Write(report.StringFormatWithoutPipe(" ", 25, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 34, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 10, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 10, 2) + "  ");
                sw.Write(report.StringFormatWithoutPipe(" ", 10, 2));
                sw.Write(report.StringFormatWithoutPipe("Total", 10, 2));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Convert.ToString(Payment)), 12, 1) + "  ");
                sw.Write(report.StringFormatWithoutPipe("", 14, 2));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Convert.ToString(Ceral)), 12, 1));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Convert.ToString(NonCeral)), 12, 1));
                sw.WriteLine("");
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message +" " + ex.StackTrace);
            }
        }
    }
}
