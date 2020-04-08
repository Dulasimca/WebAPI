using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Controllers.Reports.DailyStock;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class StockStatementByGodown
    {
        ManageReport manageReport = new ManageReport();
        public List<DailyStockDetailsEntity> ProcessStockStatement(StockParameterForCommodity stockParameter)
        {
            decimal _BookBalanceWeight = 0, _PhysicalBalanceWeight = 0, _CumulitiveShortage = 0, _Shortage = 0;
            decimal _receiptuptoYesterday = 0, _receipttotay = 0, _issuesuptoYesterday = 0,
                _issuestoday = 0, _otherIssuesuptoYesterday = 0, _otherIssuestoday = 0, _writeOFFAll = 0,
                _openingBookBalance = 0, _closingBookBalance = 0, _phycialbalance = 0;
            string _itemCode = string.Empty;
            string _ITDescription = string.Empty;
            List<DailyStockDetailsEntity> dailyStockDetailsEntities = new List<DailyStockDetailsEntity>();
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                DataSet dataSetMaster = new DataSet();
                List<KeyValuePair<string, string>> _mastersqlParameters = new List<KeyValuePair<string, string>>();
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FromDate));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@ITCode", stockParameter.CommodityCode));
                dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataToProcessCBForGodown", _mastersqlParameters);
                DataSet todayIssues = new DataSet();
                DataSet todayReceipt = new DataSet();
                DataSet issuesUptoYesterday = new DataSet();
                DataSet receiptUptoYesterday = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();

                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FromDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", stockParameter.ToDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", stockParameter.CommodityCode));
                todayIssues = manageSQLConnection.GetDataSetValues("GetTodayIssuesForGodown", sqlParameters);
                todayReceipt = manageSQLConnection.GetDataSetValues("GetTodayReceiptForGodown", sqlParameters);
                issuesUptoYesterday = manageSQLConnection.GetDataSetValues("GetIssuesUptoYesterdayForGodown", sqlParameters);
                receiptUptoYesterday = manageSQLConnection.GetDataSetValues("GetReceiptUptoYesterdayForGodown", sqlParameters);
                if (dataSetMaster.Tables.Count > 0)
                {
                    foreach (DataRow godown in dataSetMaster.Tables[2].Rows) // godown details.
                    {
                        //foreach (DataRow item in dataSetMaster.Tables[1].Rows) // item master details.
                        //{
                            DailyStockDetailsEntity stockDetailsEntity = new DailyStockDetailsEntity();
                            _itemCode = stockParameter.CommodityCode;
                            _ITDescription = stockParameter.CommodityName;

                            stockDetailsEntity.ItemCode = _itemCode;
                            stockDetailsEntity.ITDescription = _ITDescription;
                            stockDetailsEntity.GodownCode = Convert.ToString(godown["TNCSCode"]);
                            stockDetailsEntity.RegionCode = Convert.ToString(godown["RGCODE"]);
                            stockDetailsEntity.RName = Convert.ToString(godown["RGNAME"]);
                            stockDetailsEntity.GName = Convert.ToString(godown["TNCSName"]);
                           // stockDetailsEntity.TNCSCapacity = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(godown["TNCSCapacity"])));
                            // sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", _itemCode));
                            //get opening balance for particualr item.
                            DataRow[] openingBalance = dataSetMaster.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");

                            DataRow[] receiptuptoYesterday = receiptUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] receipttotay = todayReceipt.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] issuesuptoYesterday = issuesUptoYesterday.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] issuestoday = todayIssues.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] otherIssuesuptoYesterday = issuesUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] otherIssuestoday = todayIssues.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "'");

                            DataRow[] writeOFFuptoYesterday = issuesUptoYesterday.Tables[2].Select("GCode='" + stockDetailsEntity.GodownCode + "'");

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
                            if (receipttotay != null && receipttotay.Count() > 0)
                            {
                                _receipttotay = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(receipttotay[0]["TOTAL"])));
                            }
                            if (issuesuptoYesterday != null && issuesuptoYesterday.Count() > 0)
                            {
                                _issuesuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(issuesuptoYesterday[0]["TOTAL"])));
                            }
                            if (issuestoday != null && issuestoday.Count() > 0)
                            {
                                _issuestoday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(issuestoday[0]["TOTAL"])));
                            }
                            if (otherIssuesuptoYesterday != null && otherIssuesuptoYesterday.Count() > 0)
                            {
                                _otherIssuesuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(otherIssuesuptoYesterday[0]["TOTAL"])));
                            }
                            if (otherIssuestoday != null && otherIssuestoday.Count() > 0)
                            {
                                _otherIssuestoday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(otherIssuestoday[0]["TOTAL"])));
                            }
                            if (writeOFFuptoYesterday.Count() > 0)
                            {
                                _writeOFFAll = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(writeOFFuptoYesterday[0]["TOTAL"])));
                            }
                            _openingBookBalance = (_BookBalanceWeight + _receiptuptoYesterday) - (_issuesuptoYesterday + _otherIssuesuptoYesterday);
                            _closingBookBalance = (_openingBookBalance + _receipttotay) - (_issuestoday + _otherIssuestoday);
                            _CumulitiveShortage = _CumulitiveShortage - _writeOFFAll;
                            _phycialbalance = _closingBookBalance - (_CumulitiveShortage + _Shortage);
                            _Shortage = _CumulitiveShortage > 0 ? _Shortage : (_CumulitiveShortage + _Shortage);
                            _CumulitiveShortage = _CumulitiveShortage > 0 ? _CumulitiveShortage : 0;
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
                        dailyStockDetailsEntities.Add(stockDetailsEntity);

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" Process Daily Stock " + ex.Message);
            }
            //ManageStockStatement manageStockStatement = new ManageStockStatement();
            //Task.Run(() => manageStockStatement.GenerateStockStatementReport(dailyStockDetailsEntities, stockParameter)); //Generate the Report
            return dailyStockDetailsEntities;
        }
    }
}
