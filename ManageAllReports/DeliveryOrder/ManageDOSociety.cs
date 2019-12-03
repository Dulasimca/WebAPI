using System;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports.DeliveryOrder
{
    public class ManageDOSociety
    {
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDOSocietyScheme(CommonEntity entity)
        {

            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.DOSocietyReportFileName;
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
            sw.WriteLine("   Godown : " + entity.GName + "Delivery Order Details Society Wise with Issue Details");
            sw.WriteLine("   D.Ord Date:" + report.FormatDate(entity.FromDate) + "           To : " + report.FormatDate(entity.Todate));
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine(" Godown Name             DONO               SOCIETY                  D0 DATE  AMOUNTDUE      CHEQUE/DD       Adv.Col       DEB.BAL");
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void SocietyWiseDOAllSchemeReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 8;

            int i = 1;
            string doNo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool isDataAvailable = false;
            decimal Due = 0;
            decimal Paid = 0;
            decimal AdvanceCollection = 0;
            decimal Debit = 0;

            decimal GrandTotal_Due = 0;
            decimal GrandTotal_Paid = 0;
            decimal GrandTotal_ADV = 0;
            decimal GrandTotal_Debit = 0;

            try
            {
                // dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, new string[] { "Coop", "Comodity" });
                var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "issuername");
                AddHeader(sw, entity);
                foreach (DataRow date in dateList.Rows)
                {
                    Due = 0;
                    Paid = 0;
                    AdvanceCollection = 0;
                    Debit = 0;
                    isDataAvailable = true;

                    string doNoNext = string.Empty;
                    string coop = string.Empty;
                    coop = Convert.ToString(date["issuername"]);
                    DataRow[] data = entity.dataSet.Tables[0].Select("issuername='" + coop + "'");
                    i = 1;
                    foreach (DataRow row in data)
                    {
                        if (count >= 50)
                        {
                            //Add header again
                            count = 8;
                            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                            sw.WriteLine((char)12);
                            AddHeader(sw, entity);
                        }
                        sw.Write(report.StringFormatWithoutPipe(row["GodownName"].ToString(), 25, 2));

                        sw.Write(report.StringFormatWithoutPipe(row["Dono"].ToString(), 10, 2));
                        sw.Write(report.StringFormatWithoutPipe(i == 1 ? row["issuername"].ToString() : " ", 30, 2));
                        sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(row["DoDate"].ToString()), 10, 2));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Due"].ToString()), 13, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Paid"].ToString()), 12, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["AdvanceCollection"].ToString()), 13, 1));
                        sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(row["Debit"].ToString()), 12, 1));
                        sw.WriteLine("");
                        Due += !string.IsNullOrEmpty(Convert.ToString(row["Due"])) ? Convert.ToDecimal(row["Due"].ToString()) : 0;
                        Paid += !string.IsNullOrEmpty(Convert.ToString(row["Paid"])) ? Convert.ToDecimal(row["Paid"].ToString()) : 0;
                        AdvanceCollection += !string.IsNullOrEmpty(Convert.ToString(row["AdvanceCollection"])) ? Convert.ToDecimal(row["AdvanceCollection"].ToString()) : 0;
                        Debit += !string.IsNullOrEmpty(Convert.ToString(row["Debit"])) ? Convert.ToDecimal(row["Debit"].ToString()) : 0;
                        i = 2;
                        count++;
                    }
                    sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                    sw.Write(report.StringFormatWithoutPipe(" ", 35, 2));
                    sw.Write(report.StringFormatWithoutPipe(" ", 30, 2));
                    sw.Write(report.StringFormatWithoutPipe("Total", 10, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Due.ToString()), 13, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Paid.ToString()), 12, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(AdvanceCollection.ToString()), 13, 1));
                    sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(Debit.ToString()), 12, 1));
                    sw.WriteLine("");
                    GrandTotal_Due += Due;
                    GrandTotal_Paid += Paid;
                    GrandTotal_ADV += AdvanceCollection;
                    GrandTotal_Debit += Debit;
                    sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                }
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                sw.Write(report.StringFormatWithoutPipe(" ", 35, 2));
                sw.Write(report.StringFormatWithoutPipe(" ", 20, 2));
                sw.Write(report.StringFormatWithoutPipe("Grand Total", 20, 2));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_Due.ToString()), 13, 1));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_Paid.ToString()), 12, 1));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_ADV.ToString()), 13, 1));
                sw.Write(report.StringFormatWithoutPipe(report.Decimalformat(GrandTotal_Debit.ToString()), 12, 1));
                sw.WriteLine("");
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                //Check Collections 
                sw.WriteLine((char)12);
                if (!isDataAvailable)
                {
                    sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
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
