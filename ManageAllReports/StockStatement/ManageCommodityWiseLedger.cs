using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class ManageCommodityWiseLedger
    {
        ManageReport manageReport = new ManageReport();
        public List<DailyStockDetailsEntity> ProcessCommodityLedger(StockParameter stockParameter)
        {
            decimal _BookBalanceWeight = 0, _PhysicalBalanceWeight = 0, _CumulitiveShortage = 0, _Shortage = 0;
            decimal _receiptuptoYesterday = 0, _receipttotay = 0, _issuesuptoYesterday = 0,
                _issuestoday = 0, _otherIssuesuptoYesterday = 0, _otherIssuestoday = 0, _writeOFFAll = 0,
                _openingBookBalance = 0, _closingBookBalance = 0, _phycialbalance = 0;
            string _itemCode = string.Empty;
            DateTime fromDate = Convert.ToDateTime(stockParameter.FDate);
            string _ITDescription = string.Empty;
            List<DailyStockDetailsEntity> dailyStockDetailsEntities = new List<DailyStockDetailsEntity>();
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                DataSet dataSetMaster = new DataSet();
                List<KeyValuePair<string, string>> _mastersqlParameters = new List<KeyValuePair<string, string>>();
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockParameter.GCode));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@RCode", stockParameter.RCode));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FDate));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stockParameter.ItemCode));
                dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataToProcessCBForLedger", _mastersqlParameters);
                DataSet todayIssues = new DataSet();
                DataSet todayReceipt = new DataSet();
                DataSet issuesUptoYesterday = new DataSet();
                DataSet receiptUptoYesterday = new DataSet();

                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stockParameter.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", stockParameter.ToDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", stockParameter.RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stockParameter.ItemCode));
                todayIssues = manageSQLConnection.GetDataSetValues("GetTodayIssuesByDateForLedger", sqlParameters);
                todayReceipt = manageSQLConnection.GetDataSetValues("GetTodayReceiptByDateForLedger", sqlParameters);
                issuesUptoYesterday = manageSQLConnection.GetDataSetValues("GetIssuesUptoYesterdayByDateForLedger", sqlParameters);
                receiptUptoYesterday = manageSQLConnection.GetDataSetValues("GetReceiptUptoYesterdayByDateForLedger", sqlParameters);
                if (dataSetMaster.Tables.Count > 0)
                {
                    foreach (DataRow godown in dataSetMaster.Tables[2].Rows) // godown details.
                    {
                        //Check date wise issues and receipt
                        // DataRow item = dataSetMaster.Tables[1].Rows;
                        _itemCode = Convert.ToString(dataSetMaster.Tables[1].Rows[0]["ITCode"]);
                        _ITDescription = Convert.ToString(dataSetMaster.Tables[1].Rows[0]["ITDescription"]);

                        int dayDiff = Convert.ToInt32(manageReport.GetDays(stockParameter.FDate, stockParameter.ToDate)) - 1;
                        decimal DailyOB = 0;
                        DataRow[] openingBalance = dataSetMaster.Tables[0].Select("GodownCode='" + stockParameter.GCode + "' and CommodityCode='" + _itemCode + "'");
                        DataRow[] issuesuptoYesterday = issuesUptoYesterday.Tables[1].Select("GCode='" + stockParameter.GCode + "' and ITCode='" + _itemCode + "'");
                        DataRow[] receiptuptoYesterday = receiptUptoYesterday.Tables[0].Select("GCode='" + stockParameter.GCode + "' and ITCode='" + _itemCode + "'");
                        DataRow[] otherIssuesuptoYesterday = issuesUptoYesterday.Tables[0].Select("GCode='" + stockParameter.GCode + "' and ITCode='" + _itemCode + "'");
                        DataRow[] writeOFFuptoYesterday = issuesUptoYesterday.Tables[2].Select("GCode='" + stockParameter.GCode + "' and ITCode='" + _itemCode + "'");

                        if (openingBalance != null && openingBalance.Count() > 0)
                        {
                            _BookBalanceWeight = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["BookBalanceWeight"])));
                            _PhysicalBalanceWeight = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["PhysicalBalanceWeight"])));
                            _CumulitiveShortage = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["CumulitiveShortage"])));
                            _Shortage = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["WriteOff"])));
                        }
                        if (receiptuptoYesterday != null && receiptuptoYesterday.Count() > 0)
                        {
                            _receiptuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(receiptuptoYesterday[0]["TOTAL"])));
                        }
                        if (issuesuptoYesterday != null && issuesuptoYesterday.Count() > 0)
                        {
                            _issuesuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(issuesuptoYesterday[0]["TOTAL"])));
                        }
                        if (otherIssuesuptoYesterday != null && otherIssuesuptoYesterday.Count() > 0)
                        {
                            _otherIssuesuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(otherIssuesuptoYesterday[0]["TOTAL"])));
                        }
                        if (writeOFFuptoYesterday.Count() > 0)
                        {
                            _writeOFFAll = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(writeOFFuptoYesterday[0]["TOTAL"])));
                        }
                        DailyOB = (_BookBalanceWeight + _receiptuptoYesterday) - (_issuesuptoYesterday + _otherIssuesuptoYesterday);
                        for (int i = 0; i <= dayDiff; i++)
                        {
                            DateTime nDate = fromDate.AddDays(i);
                            DailyStockDetailsEntity stockDetailsEntity = new DailyStockDetailsEntity();

                            stockDetailsEntity.ItemCode = _itemCode;
                            stockDetailsEntity.ITDescription = _ITDescription;
                            stockDetailsEntity.GodownCode = Convert.ToString(godown["TNCSCode"]);
                            stockDetailsEntity.RegionCode = Convert.ToString(godown["TNCSRegn"]);
                            stockDetailsEntity.RName = Convert.ToString(godown["RGNAME"]);
                            stockDetailsEntity.GName = Convert.ToString(godown["TNCSName"]);
                            stockDetailsEntity.TNCSCapacity = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(godown["TNCSCapacity"])));
                            // sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", _itemCode));

                            //get opening balance for particualr item.

                            DataRow[] receipttotay = todayReceipt.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and DocDate='" + nDate + "'");
                            DataRow[] issuestoday = todayIssues.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and DocDate='" + nDate + "'");
                            DataRow[] otherIssuestoday = todayIssues.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "' and DocDate='" + nDate + "'");
                          
                            _BookBalanceWeight = 0;
                            _PhysicalBalanceWeight = 0;
                            _CumulitiveShortage = 0;
                            _Shortage = 0;
                            _receiptuptoYesterday = 0;
                            _receipttotay = 0;
                            _issuesuptoYesterday = 0;
                            _issuestoday = 0;
                            _otherIssuesuptoYesterday = 0;
                            _otherIssuestoday = 0;
                            _openingBookBalance = 0;
                            _closingBookBalance = 0;
                            _phycialbalance = 0;
                            _writeOFFAll = 0;
                            
                            if (receipttotay != null && receipttotay.Count() > 0)
                            {
                                _receipttotay = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(receipttotay[0]["TOTAL"])));
                            }
                          
                            if (issuestoday != null && issuestoday.Count() > 0)
                            {
                                _issuestoday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(issuestoday[0]["TOTAL"])));
                            }
                           
                            if (otherIssuestoday != null && otherIssuestoday.Count() > 0)
                            {
                                _otherIssuestoday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(otherIssuestoday[0]["TOTAL"])));
                            }
                           
                            _openingBookBalance = DailyOB;
                            _closingBookBalance = (_openingBookBalance + _receipttotay) - (_issuestoday + _otherIssuestoday);
                            _CumulitiveShortage = _CumulitiveShortage - _writeOFFAll;
                          //  _phycialbalance = _closingBookBalance - (_CumulitiveShortage + _Shortage);
                            _Shortage = _CumulitiveShortage > 0 ? _Shortage : (_CumulitiveShortage + _Shortage);
                            _CumulitiveShortage = _CumulitiveShortage > 0 ? _CumulitiveShortage : 0;
                            stockDetailsEntity.DocDate = manageReport.FormatIndianDate(nDate);
                            stockDetailsEntity.OpeningBalance = _openingBookBalance;
                            stockDetailsEntity.ClosingBalance = _closingBookBalance;
                            stockDetailsEntity.PhycialBalance = _phycialbalance;
                            stockDetailsEntity.CSBalance = _CumulitiveShortage;
                            stockDetailsEntity.Shortage = _Shortage;
                            stockDetailsEntity.TotalReceipt = _receipttotay;
                            stockDetailsEntity.IssueSales = _issuestoday;
                            stockDetailsEntity.IssueOthers = _otherIssuestoday;
                            stockDetailsEntity.LastUpdated = DateTime.Now;
                            stockDetailsEntity.Remarks = string.Empty;
                            stockDetailsEntity.Flag = true;
                            // InsertData(stockDetailsEntity);
                            //}
                            // Data checking
                            decimal CheckData = stockDetailsEntity.TotalReceipt + stockDetailsEntity.IssueSales
                                                 + stockDetailsEntity.IssueOthers;
                            if (CheckData > 0)
                            {
                                dailyStockDetailsEntities.Add(stockDetailsEntity);
                            }
                            DailyOB = _closingBookBalance;
                        }
                        //foreach (DataRow item in dataSetMaster.Tables[1].Rows) // item master details.
                        //{
                           
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" Process Daily Stock " + ex.Message);
            }
            //  ManageStockStatement manageStockStatement = new ManageStockStatement();
            // Task.Run(() => manageStockStatement.GenerateStockStatementReport(dailyStockDetailsEntities, stockParameter)); //Generate the Report
            return dailyStockDetailsEntities;
        }
    }
}
