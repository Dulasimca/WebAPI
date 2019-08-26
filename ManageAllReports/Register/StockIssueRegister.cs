using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace TNCSCAPI.ManageAllReports
{
    public class StockIssueRegister
    {
        private string GName { get; set; }
        private string RName { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();
        /// <summary>
        /// Generate the stock issue register report.
        /// </summary>
        /// <param name="entity">Common entity</param>
        public void GenerateStockIssuesRegister(CommonEntity entity)
        {
            AuditLog.WriteError("GenerateIssuesRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                GName = entity.dataSet.Tables[0].Rows[0]["Godownname"].ToString();
                RName = entity.dataSet.Tables[0].Rows[0]["Region"].ToString();
                fileName = entity.GCode + GlobalVariable.StockIssueRegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseStockIssuesRegister(streamWriter, entity);

                List<StockIssuesEntity> stockIssuesList = new List<StockIssuesEntity>();
                stockIssuesList = report.ConvertDataTableToList<StockIssuesEntity>(entity.dataSet.Tables[0]);
                StockIssuesAbstract(streamWriter, stockIssuesList, entity);
                StockIssuesSchemewiseAbstract(streamWriter, stockIssuesList, entity);
                StockIssuesReceiverandSchemeAbstract(streamWriter, stockIssuesList, entity);
                StockIssuesCommoditywiseAbstract(streamWriter, stockIssuesList, entity);
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
        /// Add date wise stock issues
        /// </summary>
        /// <param name="sw">Stream writer</param>
        /// <param name="entity">Common eitity</param>
        public void DateWiseStockIssuesRegister(StreamWriter sw, CommonEntity entity)
        {
            int count = 10;
            var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "Issue_Date");
            int i = 0;
            string issueMemo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool CheckRepeatValue = false;
            string weighmentType = string.Empty;
            foreach (DataRow date in dateList.Rows)
            {
                count = 11;
                string issueMemoNext = string.Empty;
                DataRow[] data = entity.dataSet.Tables[0].Select("Issue_Date='" + date["Issue_Date"] + "'");
                AddHeaderForDateWise(sw, Convert.ToString(date["Issue_Date"]));
                foreach (DataRow row in data)
                {
                    if (count >= 50)
                    {
                        //Add header again
                        count = 11;
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine((char)12);
                        AddHeaderForDateWise(sw, Convert.ToString(date["Issue_Date"]));
                    }
                    weighmentType = row["ITBweighment"].ToString().ToUpper();
                    issueMemoNext = row["Issue_Memono"].ToString();
                    fromWhomRcd = Convert.ToString(row["To_Whom_Issued"]).Trim();
                    if (issueMemo == issueMemoNext)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        issueMemo = issueMemoNext;
                    }
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? i.ToString() : " ", 4, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? issueMemoNext : " ", 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? row["DNo"].ToString() : " ", 11, 2));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? row["Lorryno"].ToString() : " ", 11, 1));
                    sw.Write(report.StringFormatWithoutPipe(CheckRepeatValue == false ? fromWhomRcd : " ", 33, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Scheme"].ToString(), 12, 2));
                    sw.Write(report.StringFormatWithoutPipe(row["Stackno"].ToString(), 12, 1));
                    sw.Write(report.StringFormatWithoutPipe(row["NoPacking"].ToString(), 8, 1));
                    sw.Write(report.StringFormatWithoutPipe(row["Commodity"].ToString(), 17, 2));
                    sw.Write(report.StringFormatWithoutPipe(weighmentType == "NOS" ? row["NetWt"].ToString() : report.DecimalformatForWeight(row["NetWt"].ToString()), 14, 1));
                    sw.WriteLine("");
                    i = CheckRepeatValue == false ? i + 1 : i;
                    count++;
                }
                sw.WriteLine((char)12);
            }
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            if (count == 11)
            {
                sw.WriteLine((char)12);
            }
        }

        /// <summary>
        /// Add header for date wise report
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="date"></param>
        public void AddHeaderForDateWise(StreamWriter sw, string date)
        {
            sw.WriteLine("                                  TAMILNADU CIVIL SUPPLIES CORPORATION                      Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Stock Issue Register");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(date) + "  (Net Wt in kgs\\Klts\\Nos)     Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("S.No Issue Memo  D.No        Lorry No   To Whom Issued                    Scheme       StackNo      No bags  Commodity               Net wt  ");
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("");

        }

        /// <summary>
        /// Abstract details for Stock issues
        /// </summary>
        /// <param name="sw">Streamwriter</param>
        /// <param name="stockIssues">Stock issues entity</param>
        /// <param name="entity">Common entity</param>
        public void StockIssuesAbstract(StreamWriter sw, List<StockIssuesEntity> stockIssues, CommonEntity entity)
        {
            int count = 11;
            string weighmentType = string.Empty;
            var resultSet = from d in stockIssues
                            group d by new { d.Stackno, d.Commodity, d.ITBweighment } into groupedData
                            select new
                            {
                                NoPacking = groupedData.Sum(s => s.NoPacking),
                                NetWt = groupedData.Sum(s => s.NetWt),
                                GroupByNames = groupedData.Key
                            };
            AddheaderForAbstract(sw, entity);
            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddheaderForAbstract(sw, entity);
                }
                weighmentType = item.GroupByNames.ITBweighment.ToUpper();
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Stackno, 13, 2));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 35, 2));
                sw.Write(report.StringFormatWithoutPipe(item.NoPacking.ToString(), 12, 2));
                sw.Write(report.StringFormatWithoutPipe(weighmentType == "NOS" ? item.NetWt.ToString() : report.Decimalformat(item.NetWt.ToString()), 20, 1));
                sw.WriteLine("");
                sw.WriteLine(" ");
                count = count + 2;
            }
            sw.WriteLine("------------------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for abstract report
        /// </summary>
        /// <param name="sw">streamwriter</param>
        /// <param name="entity">common entity</param>
        public void AddheaderForAbstract(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                      TAMILNADU CIVIL SUPPLIES CORPORATION                Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                 Stock Issue Register Abstract");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(entity.FromDate) + "Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("------------------------------------------------------------------------------------------");
            sw.WriteLine("StackNo       Commodity                           No Bags      Net Wt (in Kgs)\\Nos");
            sw.WriteLine("------------------------------------------------------------------------------------------");
            sw.WriteLine("");
        }

        /// <summary>
        /// Add scheme wise abstract details
        /// </summary>
        /// <param name="sw">streamwriter</param>
        /// <param name="stockIssues">stock issues eentity</param>
        /// <param name="entity">common entity</param>
        public void StockIssuesSchemewiseAbstract(StreamWriter sw, List<StockIssuesEntity> stockIssues, CommonEntity entity)
        {
            int count = 11;
            string weighmentType = string.Empty;
            var resultSet = from d in stockIssues
                            group d by new { d.Scheme, d.Commodity, d.ITBweighment, d.PName } into groupedData
                            select new
                            {
                                NoPacking = groupedData.Sum(s => s.NoPacking),
                                NetWt = groupedData.Sum(s => s.NetWt),
                                GroupByNames = groupedData.Key
                            };
            AddheaderForSchemeAbstract(sw, entity);
            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddheaderForSchemeAbstract(sw, entity);
                }
                weighmentType = item.GroupByNames.ITBweighment.ToUpper();
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Scheme, 13, 2));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 35, 2));
                sw.Write(report.StringFormatWithoutPipe(item.NoPacking.ToString(), 12, 2));
                sw.Write(report.StringFormatWithoutPipe(weighmentType == "NOS" ? item.NetWt.ToString() : report.Decimalformat(item.NetWt.ToString()), 20, 1));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.PName, 35, 2));
                sw.WriteLine("");
                sw.WriteLine(" ");
                count = count + 2;
            }
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for scheme wise abstract
        /// </summary>
        /// <param name="sw">streamwriter</param>
        /// <param name="entity">common entity</param>
        public void AddheaderForSchemeAbstract(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                      TAMILNADU CIVIL SUPPLIES CORPORATION                Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                 Stock Issue Register Scheme Wise Abstract");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(entity.FromDate) + "Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("Scheme        Commodity                           No Bags      Net Wt (in Kgs)\\Nos");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("");
        }

        /// <summary>
        /// Add receiver and scheme wise abstract details 
        /// </summary>
        /// <param name="sw">Streamwriter</param>
        /// <param name="stockIssues">Stock issues entity</param>
        /// <param name="entity">Common entity</param>
        public void StockIssuesReceiverandSchemeAbstract(StreamWriter sw, List<StockIssuesEntity> stockIssues, CommonEntity entity)
        {
            int count = 11;
            string weighmentType = string.Empty;
            var resultSet = from d in stockIssues
                            group d by new { d.To_Whom_Issued, d.Scheme, d.Commodity, d.ITBweighment, d.PName } into groupedData
                            select new
                            {
                                NoPacking = groupedData.Sum(s => s.NoPacking),
                                NetWt = groupedData.Sum(s => s.NetWt),
                                GroupByNames = groupedData.Key
                            };
            AddheaderForReceiverTypeAndSchemewiseAbstract(sw, entity);
            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddheaderForReceiverTypeAndSchemewiseAbstract(sw, entity);
                }
                weighmentType = item.GroupByNames.ITBweighment.ToUpper();
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.To_Whom_Issued, 35, 2));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Scheme, 13, 2));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 27, 2));
                sw.Write(report.StringFormatWithoutPipe(item.NoPacking.ToString(), 8, 2));
                sw.Write(report.StringFormatWithoutPipe(weighmentType == "NOS" ? item.NetWt.ToString() : report.Decimalformat(item.NetWt.ToString()), 15, 1));
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.PName.ToString(), 27, 2));
                sw.WriteLine("");
                sw.WriteLine(" ");
                count = count + 2;
            }
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for receiver and scheme wise abstract details 
        /// </summary>
        /// <param name="sw">streamwriter</param>
        /// <param name="entity">common entity</param>
        public void AddheaderForReceiverTypeAndSchemewiseAbstract(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                      TAMILNADU CIVIL SUPPLIES CORPORATION                Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                 Stock Issue Register Scheme Wise Abstract");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(entity.FromDate) + "Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("ISSUER TYPE                         Scheme        Commodity                   No Bags       Wt(Kgs\\Nos)");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("");
        }

        /// <summary>
        /// Add Commodity wise abstract details 
        /// </summary>
        /// <param name="sw">Streamwriter</param>
        /// <param name="stockIssues">Stock issues entity</param>
        /// <param name="entity">Common entity</param>
        public void StockIssuesCommoditywiseAbstract(StreamWriter sw, List<StockIssuesEntity> stockIssues, CommonEntity entity)
        {
            int count = 11;
            string weighmentType = string.Empty;
            var resultSet = from d in stockIssues
                            group d by new { d.Commodity,d.ITBweighment, d.PName } into groupedData
                            select new
                            {
                                NoPacking = groupedData.Sum(s => s.NoPacking),
                                NetWt = groupedData.Sum(s => s.NetWt),
                                GroupByNames = groupedData.Key
                            };
            AddheaderForCommoditywiseAbstract(sw, entity);
            foreach (var item in resultSet)
            {
                if (count >= 50)
                {
                    count = 11;
                    sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                    AddheaderForCommoditywiseAbstract(sw, entity);
                }
                weighmentType = item.GroupByNames.ITBweighment.ToUpper();
                sw.Write(report.StringFormatWithoutPipe(item.GroupByNames.Commodity, 36, 2));
                sw.Write(report.StringFormatWithoutPipe(item.NoPacking.ToString(), 7, 2));
                sw.Write(report.StringFormatWithoutPipe(weighmentType == "NOS" ? item.NetWt.ToString() : report.Decimalformat(item.NetWt.ToString()), 17, 1));
                sw.Write(report.StringFormatWithoutPipe( item.GroupByNames.PName, 30, 2));
                sw.WriteLine("");
                sw.WriteLine(" ");
                count = count + 2;
            }
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Add header for Commodity wise abstract details
        /// </summary>
        /// <param name="sw">streamwriter</param>
        /// <param name="entity">common entity</param>
        public void AddheaderForCommoditywiseAbstract(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                      TAMILNADU CIVIL SUPPLIES CORPORATION                Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                 Stock Issue Register COMMODITY Wise Abstract ");
            sw.WriteLine(" ");
            sw.WriteLine("Issue Date:" + report.FormatDate(entity.FromDate) + "Godown : " + GName + "          Region :" + RName);
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("         Commodity                   No Bags      Net Wt (in Kgs)\\Nos");
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("");
        }
    }

    public class StockIssuesEntity
    {
        public DateTime Issue_Date { get; set; }
        public string Region { get; set; }
        public string Godownname { get; set; }
        public string Issue_Memono { get; set; }
        public string DNo { get; set; }
        public string Lorryno { get; set; }
        public string To_Whom_Issued { get; set; }
        public string Scheme { get; set; }
        public string Stackno { get; set; }
        public int? NoPacking { get; set; }
        public string Commodity { get; set; }
        public double? NetWt { get; set; }
        public string ITBweighment { get; set; }
        public string PName { get; set; }
    }

}
