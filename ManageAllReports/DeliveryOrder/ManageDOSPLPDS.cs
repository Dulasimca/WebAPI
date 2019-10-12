using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.DeliveryOrder
{
    public class ManageDOSPLPDS
    {
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDOSPLPDSScheme(CommonEntity entity)
        {

            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.DOSPLPDSReportFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                SocietyWiseDOAllSchemeReport(streamWriter, entity);

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
            sw.WriteLine("   TAMILNADU CIVIL SUPPLIES CORPORATION          " + entity.RName);
            sw.WriteLine("   Godown : " + entity.GName + "Delivery Order Details Society Wise with Issue Details For SPL PDS ");
            sw.WriteLine("   D.Ord Date:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("D.O.No     D.O.Date  COMMODITY      SCHEME          NET.WT      Rate       C.AMOUNT  NC.AMOUNT     AMOUNT");
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void SocietyWiseDOAllSchemeReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 10;

            int i = 1;
            string doNo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool isDataAvailable = false;
            decimal Qty = 0;
            decimal Rate = 0;
            decimal C_Amount = 0;
            decimal NC_Amount = 0;
            decimal Toatal_C_Amount = 0;
            decimal Total_NC_Amount = 0;
            decimal Amount = 0;

            decimal GrandTotal_C = 0;
            decimal GrandTotal_NC = 0;
            decimal GrandTotal = 0;
            count = 11;
            try
            {
                // dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, new string[] { "Coop", "Comodity" });
                var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Coop");

                foreach (DataRow date in dateList.Rows)
                {
                    Qty = 0;
                    Rate = 0;
                    C_Amount = 0;
                    NC_Amount = 0;
                    Amount = 0;
                    Toatal_C_Amount = 0;
                    Total_NC_Amount = 0;
                    GrandTotal_C = 0;
                    GrandTotal_NC = 0;
                    GrandTotal = 0;
                    isDataAvailable = true;

                    string doNoNext = string.Empty;
                    string coop = string.Empty;
                    AddHeader(sw, entity);
                    coop = Convert.ToString(date["Coop"]);
                    DataRow[] data = entity.dataSet.Tables[0].Select("Coop='" + coop + "'");
                    var distinctCommodity = report.ConvertDataRowToTable(data, entity.dataSet.Tables[0]).DefaultView.ToTable(true, "Comodity");
                    sw.WriteLine(report.StringFormatWithoutPipe(coop, 50, 2));

                    foreach (DataRow item in distinctCommodity.Rows)
                    {
                        Qty = 0;
                        Rate = 0;
                        C_Amount = 0;
                        NC_Amount = 0;
                        Amount = 0;
                        Toatal_C_Amount = 0;
                        Total_NC_Amount = 0;
                        DataRow[] ndata = entity.dataSet.Tables[0].Select("Coop='" + coop + "' and Comodity='" + Convert.ToString(item["Comodity"]) + "'");
                        foreach (DataRow row in ndata)
                        {
                            C_Amount = 0;
                            NC_Amount = 0;
                            if (count >= 50)
                            {
                                //Add header again
                                count = 11;
                                sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
                                sw.WriteLine((char)12);
                                AddHeader(sw, entity);
                            }
                            doNoNext = row["Dono"].ToString();
                            C_Amount = (row["C_Nc"].ToString() == "C") ? Convert.ToDecimal(row["Amount"]) : 0;
                            NC_Amount = (row["C_Nc"].ToString() == "NC") ? Convert.ToDecimal(row["Amount"]) : 0;
                            sw.Write(report.StringFormatWithoutPipe(doNoNext, 11, 1));
                            sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(row["Dodate"].ToString()), 10, 2));
                            sw.Write(report.StringFormatWithoutPipe(row["Comodity"].ToString(), 15, 2));
                            sw.Write(report.StringFormatWithoutPipe(row["Scheme"].ToString(), 11, 2));
                            sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(row["Quantity"].ToString()), 11, 1));
                            sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Rate"].ToString()), 10, 1));
                            sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Convert.ToString(C_Amount)), 11, 1));
                            sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Convert.ToString(NC_Amount)), 11, 1));
                            sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Amount"].ToString()), 11, 1));
                            sw.WriteLine("");
                            Rate += !string.IsNullOrEmpty(Convert.ToString(row["Rate"])) ? Convert.ToDecimal(row["Rate"].ToString()) : 0;
                            Toatal_C_Amount += !string.IsNullOrEmpty(Convert.ToString(C_Amount)) ? C_Amount : 0;
                            Total_NC_Amount += !string.IsNullOrEmpty(Convert.ToString(NC_Amount)) ? NC_Amount : 0;
                            Amount += !string.IsNullOrEmpty(Convert.ToString(row["Amount"])) ? Convert.ToDecimal(row["Amount"].ToString()) : 0;
                            Qty += !string.IsNullOrEmpty(Convert.ToString(row["Quantity"])) ? Convert.ToDecimal(row["Quantity"].ToString()) : 0;
                            i = i + 1;
                            count++;
                        }
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
                        sw.Write(report.StringFormatWithoutPipe("", 11, 2));
                        sw.Write(report.StringFormatWithoutPipe("", 10, 2));
                        sw.Write(report.StringFormatWithoutPipe("", 15, 2));
                        sw.Write(report.StringFormatWithoutPipe("  Total ", 11, 1));
                        sw.Write(report.StringFormatWithoutPipe(Qty.ToString(), 11, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Rate.ToString()), 10, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Toatal_C_Amount.ToString()), 11, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Total_NC_Amount.ToString()), 11, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Amount.ToString()), 11, 1));
                        GrandTotal_C += Toatal_C_Amount;
                        GrandTotal_NC += Total_NC_Amount;
                        GrandTotal += Amount;
                        sw.WriteLine("");
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    }

                    sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    sw.Write(report.StringFormatWithoutPipe("", 11, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 10, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 15, 2));
                    sw.Write(report.StringFormatWithoutPipe("  Demand ", 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(" ", 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(" ", 10, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_C.ToString()), 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_NC.ToString()), 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal.ToString()), 11, 1));
                    sw.WriteLine("");
                    sw.WriteLine("---------------------------------------------------------------------------------------------------------------");

                    //Check Collections 
                    sw.WriteLine((char)12);
                }
                if (!isDataAvailable)
                {
                    sw.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
