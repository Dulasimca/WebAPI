using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class StockStatementByDate
    {
        public List<DailyStockDetailsEntity> ProcessStockStatement(StockParameter stockParameter)
        {
            float _BookBalanceWeight = 0, _PhysicalBalanceWeight = 0, _CumulitiveShortage = 0, _Shortage = 0;
            float _receiptuptoYesterday = 0, _receipttotay = 0, _issuesuptoYesterday = 0,
                _issuestoday = 0, _otherIssuesuptoYesterday = 0, _otherIssuestoday = 0,
                _openingBookBalance = 0, _closingBookBalance = 0, _phycialbalance = 0;
            string _itemCode = string.Empty;
            string _ITDescription = string.Empty;
            List<DailyStockDetailsEntity> dailyStockDetailsEntities = new List<DailyStockDetailsEntity>();
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                DataSet dataSetMaster = new DataSet();
              
               dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataToProcessCB");
                if (dataSetMaster.Tables.Count > 0)
                {
                    foreach (DataRow item in dataSetMaster.Tables[1].Rows) // item master details.
                    {
                        DataSet todayIssues = new DataSet();
                        DataSet todayReceipt = new DataSet();
                        DataSet issuesUptoYesterday = new DataSet();
                        DataSet receiptUptoYesterday = new DataSet();

                        _itemCode = Convert.ToString(item["ITCode"]);
                        _ITDescription = Convert.ToString(item["ITDescription"]);
                        List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                        DailyStockDetailsEntity stockDetailsEntity = new DailyStockDetailsEntity();
                        stockDetailsEntity.ItemCode = _itemCode;
                        stockDetailsEntity.ITDescription = _ITDescription;
                        stockDetailsEntity.GodownCode = stockParameter.GCode;
                        stockDetailsEntity.RegionCode = stockParameter.RCode;

                        sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", _itemCode));
                        sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stockDetailsEntity.GodownCode));
                        sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FDate));
                        sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", stockParameter.ToDate));
                        todayIssues = manageSQLConnection.GetDataSetValues("GetTodayIssuesByDate", sqlParameters);
                        todayReceipt = manageSQLConnection.GetDataSetValues("GetTodayReceiptByDate", sqlParameters);
                        issuesUptoYesterday = manageSQLConnection.GetDataSetValues("GetIssuesUptoYesterdayByDate", sqlParameters);
                        receiptUptoYesterday = manageSQLConnection.GetDataSetValues("GetReceiptUptoYesterdayByDate", sqlParameters);
                        //foreach (DataRow godown in dataSetMaster.Tables[2].Rows) // godown details.
                        //{
                            
                            //get opening balance for particualr item.
                            DataRow[] openingBalance = dataSetMaster.Tables[0].Select("GodownCode='" + stockDetailsEntity.GodownCode + "' and CommodityCode='" + stockDetailsEntity.ItemCode + "'");

                            DataRow[] receiptuptoYesterday = receiptUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] receipttotay = todayReceipt.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] issuesuptoYesterday = issuesUptoYesterday.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] issuestoday = todayIssues.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'");
                            DataRow[] otherIssuesuptoYesterday = issuesUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "'"); ;
                            DataRow[] otherIssuestoday = todayIssues.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "'");

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
                            if (openingBalance != null && openingBalance.Count() > 0)
                            {
                                _BookBalanceWeight = float.Parse(Convert.ToString(openingBalance[0]["BookBalanceWeight"]));
                                _PhysicalBalanceWeight = float.Parse(Convert.ToString(openingBalance[0]["PhysicalBalanceWeight"]));
                                _CumulitiveShortage = float.Parse(Convert.ToString(openingBalance[0]["CumulitiveShortage"]));
                                _Shortage = float.Parse(Convert.ToString(openingBalance[0]["WriteOff"]));
                            }
                            if (receiptuptoYesterday != null && receiptuptoYesterday.Count() > 0)
                            {
                                _receiptuptoYesterday = float.Parse(Convert.ToString(receiptuptoYesterday[0]["TOTAL"]));
                            }
                            if (receipttotay != null && receipttotay.Count() > 0)
                            {
                                _receipttotay = float.Parse(Convert.ToString(receipttotay[0]["TOTAL"]));
                            }
                            if (issuesuptoYesterday != null && issuesuptoYesterday.Count() > 0)
                            {
                                _issuesuptoYesterday = float.Parse(Convert.ToString(issuesuptoYesterday[0]["TOTAL"]));
                            }
                            if (issuestoday != null && issuestoday.Count() > 0)
                            {
                                _issuestoday = float.Parse(Convert.ToString(issuestoday[0]["TOTAL"]));
                            }
                            if (otherIssuesuptoYesterday != null && otherIssuesuptoYesterday.Count() > 0)
                            {
                                _otherIssuesuptoYesterday = float.Parse(Convert.ToString(otherIssuesuptoYesterday[0]["TOTAL"]));
                            }
                            if (otherIssuestoday != null && otherIssuestoday.Count() > 0)
                            {
                                _otherIssuestoday = float.Parse(Convert.ToString(otherIssuestoday[0]["TOTAL"]));
                            }
                            _openingBookBalance = (_BookBalanceWeight + _receiptuptoYesterday) - (_issuesuptoYesterday + _otherIssuesuptoYesterday);
                            _closingBookBalance = (_openingBookBalance + _receipttotay) - (_issuestoday + _otherIssuestoday);
                            _phycialbalance = _closingBookBalance - (_CumulitiveShortage + _Shortage);
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
                    }
                }
            }
            catch (Exception ex)
            {
              AuditLog.WriteError(" Process Daily Stock " + ex.Message);
            }
            return dailyStockDetailsEntities;
        }
    }
    public class DailyStockDetailsEntity
    {
        public string ItemCode { get; set; }
        public string ITDescription { get; set; }
        public float OpeningBalance { get; set; }
        public float PhycialBalance { get; set; }
        public float ClosingBalance { get; set; }
        public float CSBalance { get; set; }
        public float Shortage { get; set; }
        public float TotalReceipt { get; set; }
        public float IssueSales { get; set; }
        public float IssueOthers { get; set; }
        public DateTime LastUpdated { get; set; }
        public string GodownCode { get; set; }
        public string RegionCode { get; set; }
        public string Remarks { get; set; }
        public bool Flag { get; set; }
    }

    public class StockParameter
    {
        public string FDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string UserName { get; set; }
    }
}
