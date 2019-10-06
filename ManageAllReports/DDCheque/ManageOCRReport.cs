using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.DDCheque
{
    public class ManageOCRReport
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateOCRReport(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.OCRReportFileName;
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
            sw.WriteLine("  TamilNadu Civil Supplies Corporation                            DAILY CASH RECEIPT Account Register" );
            sw.WriteLine(" ");
            sw.WriteLine("  Godown Name : "+ entity.GName + "                         Region Name : " + entity.RName);
            sw.WriteLine(" ");
            sw.WriteLine(" From:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine(" S.No Depositor Name                           Receipt no & Date      DD/CH No              Date      Amount         BANK");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
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
                int i = 1;
                AddHeader(sw, entity);
                decimal TotalAmount = 0;
                foreach (DataRow row in entity.dataSet.Tables[0].Rows)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 11;
                        sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeader(sw, entity);
                    }
                    sw.Write(" ");
                    sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["ReceivedFrom"].ToString(), 40, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["ReceiptNo"].ToString(), 11, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(row["Date"].ToString()), 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["PaymentType"].ToString(), 3, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["DDNo"].ToString(), 15, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["DDDate"].ToString(), 10, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Amount"].ToString()), 15, 1));
                    sw.Write(report.StringFormatWithoutPipe(row["Bank"].ToString(), 32, 2));
                     sw.WriteLine("");
                    TotalAmount += Convert.ToDecimal(report.Decimalformat(row["Amount"].ToString()));
                    i = i + 1;
                    count = count + 1;
                }
                sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.Write("                                                                TOTAL AMOUNT                       ");
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(TotalAmount.ToString()), 15, 1));
                sw.WriteLine("");
                sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}

