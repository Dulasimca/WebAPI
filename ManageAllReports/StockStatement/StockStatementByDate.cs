using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class StockStatementByDate
    {
        ManageReport manageReport = new ManageReport();
        public List<DailyStockDetailsEntity> ProcessStockStatement(StockParameter stockParameter)
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
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockParameter.GCode));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@RCode", stockParameter.RCode));
                _mastersqlParameters.Add(new KeyValuePair<string, string>("@Date", stockParameter.FDate));
                dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataToProcessCB", _mastersqlParameters);
                DataSet todayIssues = new DataSet();
                DataSet todayReceipt = new DataSet();
                DataSet issuesUptoYesterday = new DataSet();
                DataSet receiptUptoYesterday = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();


                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stockParameter.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stockParameter.FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", stockParameter.ToDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", stockParameter.RCode));
                todayIssues = manageSQLConnection.GetDataSetValues("GetTodayIssuesByDate", sqlParameters);
                todayReceipt = manageSQLConnection.GetDataSetValues("GetTodayReceiptByDate", sqlParameters);
                issuesUptoYesterday = manageSQLConnection.GetDataSetValues("GetIssuesUptoYesterdayByDate", sqlParameters);
                receiptUptoYesterday = manageSQLConnection.GetDataSetValues("GetReceiptUptoYesterdayByDate", sqlParameters);
                if (dataSetMaster.Tables.Count > 0)
                {
                    foreach (DataRow godown in dataSetMaster.Tables[2].Rows) // godown details.
                    {
                        foreach (DataRow item in dataSetMaster.Tables[1].Rows) // item master details.
                        {
                            DailyStockDetailsEntity stockDetailsEntity = new DailyStockDetailsEntity();
                            _itemCode = Convert.ToString(item["ITCode"]);
                            _ITDescription = Convert.ToString(item["ITDescription"]);

                            stockDetailsEntity.ItemCode = _itemCode;
                            stockDetailsEntity.ITDescription = _ITDescription;
                            stockDetailsEntity.GodownCode = Convert.ToString(godown["TNCSCode"]);
                            stockDetailsEntity.RegionCode = Convert.ToString(godown["TNCSRegn"]);
                            stockDetailsEntity.RName = Convert.ToString(godown["RGNAME"]);
                            stockDetailsEntity.GName = Convert.ToString(godown["TNCSName"]);
                            stockDetailsEntity.TNCSCapacity = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(godown["TNCSCapacity"])));
                            // sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", _itemCode));



                            //get opening balance for particualr item.
                            DataRow[] openingBalance = dataSetMaster.Tables[0].Select("GodownCode='" + stockDetailsEntity.GodownCode + "' and CommodityCode='" + stockDetailsEntity.ItemCode + "'");

                            DataRow[] receiptuptoYesterday = receiptUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] receipttotay = todayReceipt.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] issuesuptoYesterday = issuesUptoYesterday.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] issuestoday = todayIssues.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] otherIssuesuptoYesterday = issuesUptoYesterday.Tables[0].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] otherIssuestoday = todayIssues.Tables[1].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");

                            DataRow[] writeOFFuptoYesterday = issuesUptoYesterday.Tables[2].Select("GCode='" + stockDetailsEntity.GodownCode + "' and ITCode='" + _itemCode + "'");

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
                            // Data checking
                            decimal CheckData = stockDetailsEntity.OpeningBalance + stockDetailsEntity.ClosingBalance +
                                                 stockDetailsEntity.TotalReceipt + stockDetailsEntity.IssueSales
                                                 + stockDetailsEntity.IssueOthers;
                            if (CheckData > 0)
                            {
                                dailyStockDetailsEntities.Add(stockDetailsEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" Process Daily Stock " + ex.Message);
            }
            ManageStockStatement manageStockStatement = new ManageStockStatement();
            Task.Run(() => manageStockStatement.GenerateStockStatementReport(dailyStockDetailsEntities, stockParameter)); //Generate the Report
            return dailyStockDetailsEntities;
        }
    }
    public class DailyStockDetailsEntity
    {
        public string ItemCode { get; set; }
        public string DocDate { get; set; }
        public string GName { get; set; }
        public decimal TNCSCapacity { get; set; }
        public string RName { get; set; }
        public string ITDescription { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal PhycialBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal CSBalance { get; set; }
        public decimal Shortage { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal IssueSales { get; set; }
        public decimal IssueOthers { get; set; }
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
        public string GName { get; set; }
        public string RName { get; set; }
        public string UserName { get; set; }
        public string ItemCode { get; set; }
    }

    public class StockCBEntity
    {
        public StockCBEntity()
        {
            RNAME = string.Empty;
            TNCSName = string.Empty;
            TNCSCapacity = default(decimal); 
            GStatus = string.Empty;
            RStatus = string.Empty;
            GRemark = string.Empty;
            RRemark = string.Empty;
            PADDY_A = default(decimal);
            PADDY_COMMON = default(decimal);
            RAW_RICE_COMMON = default(decimal);
            BOILED_RICE_COMMON = default(decimal);
            SUGAR = default(decimal);
            WHEAT = default(decimal);
            RAVA = default(decimal);
            MAIDA = default(decimal);
            TOOR_DHALL = default(decimal);
            URID_DHALL = default(decimal);
            MAZOOR__DHALL = default(decimal);
            GREEN_GRAM_DHALL = default(decimal);
            GREEN_GRAM = default(decimal);
            BENGAL_GRAM = default(decimal);
            PALMOLIEN_OIL = default(decimal);
            SALT = default(decimal);
            TEA = default(decimal);
            KEROSENE = default(decimal);
            RAW_RICE_A = default(decimal);
            BOILED_RICE_A = default(decimal);
            JUTE_TWINE = default(decimal);
            PALMOLIEN_POUCH = default(decimal);
            CEMENT_IMPORTED = default(decimal);
            ATTA = default(decimal);
            CEMENT_REGULAR = default(decimal);
            URID_DHALL_SPLIT = default(decimal);
            Candian_Yellow_lentil_TD = default(decimal);
            SALT_FF = default(decimal);
            RAW_RICE_A_HULLING = default(decimal);
            RAW_RICE_COM_HULLING = default(decimal);
            YELLOW_LENTAL_US = default(decimal);
            URAD_SQ = default(decimal);
            BOILED_RICE_A_HULLING = default(decimal);
            BOILED_RICE_C_HULLING = default(decimal);
            LIARD_LENTIL_GREEN = default(decimal);
            TUR_LEMON = default(decimal);
            URAD_FAQ = default(decimal);
            TUR_ARUSHA = default(decimal);
            URID_DHALL_FAQ = default(decimal);
            URID_DHALL_SQ = default(decimal);
            AMMA_SALT_RFFIS = default(decimal);
            AMMA_SALT_DFS = default(decimal);
            AMMA_SALT_LSS = default(decimal);
            AMMA_CEMENT = default(decimal);
            AMMA_SALT_CIS = default(decimal);
            OMR_HULLING = default(decimal);
            BOILED_RICE_FORTIFIED = default(decimal);
            Fortified_Rice_Kernels = default(decimal);
            FRK_Blended_Rice = default(decimal);

        }

        public string RNAME { get; set; }
        public string TNCSName { get; set; }
        public decimal TNCSCapacity { get; set; }
        public string GStatus { get; set; }
        public string RStatus { get; set; }
        public string GRemark { get; set; }
        public string RRemark { get; set; }
        public decimal PADDY_A { get; set; }//IT001
        public decimal PADDY_COMMON { get; set; }//IT003
        public decimal RAW_RICE_COMMON { get; set; }//IT009
        public decimal BOILED_RICE_COMMON { get; set; }//IT012
        public decimal SUGAR { get; set; }//IT013
        public decimal WHEAT { get; set; }//IT014
        public decimal RAVA { get; set; }//IT015
        public decimal MAIDA { get; set; }//IT016
        public decimal TOOR_DHALL { get; set; }//IT018
        public decimal URID_DHALL { get; set; }//IT019
        public decimal MAZOOR__DHALL { get; set; }//IT020
        public decimal GREEN_GRAM_DHALL { get; set; }//IT021
        public decimal GREEN_GRAM { get; set; }//IT024
        public decimal BENGAL_GRAM { get; set; }//IT025
        public decimal PALMOLIEN_OIL { get; set; }//IT026
        public decimal SALT { get; set; }//IT027
        public decimal TEA { get; set; }//IT029
        public decimal KEROSENE { get; set; }//IT030
        public decimal RAW_RICE_A { get; set; }//IT033
        public decimal BOILED_RICE_A { get; set; }//IT034
        public decimal JUTE_TWINE { get; set; }//IT041
        public decimal PALMOLIEN_POUCH { get; set; }//IT059
        public decimal CEMENT_IMPORTED { get; set; }//IT064
        public decimal ATTA { get; set; }//IT065
        public decimal CEMENT_REGULAR { get; set; }//IT066
        public decimal URID_DHALL_SPLIT { get; set; }//IT112
        public decimal Candian_Yellow_lentil_TD { get; set; }//IT120
        public decimal SALT_FF { get; set; }//IT121
        public decimal RAW_RICE_A_HULLING { get; set; }//IT123
        public decimal RAW_RICE_COM_HULLING { get; set; }//IT124
        public decimal YELLOW_LENTAL_US { get; set; }//IT132
        public decimal URAD_SQ { get; set; }//IT133
        public decimal BOILED_RICE_A_HULLING { get; set; }//IT139
        public decimal BOILED_RICE_C_HULLING { get; set; }//IT140
        public decimal LIARD_LENTIL_GREEN { get; set; }//IT166
        public decimal TUR_LEMON { get; set; }//IT175
        public decimal URAD_FAQ { get; set; }//IT176
        public decimal TUR_ARUSHA { get; set; }//IT190
        public decimal URID_DHALL_FAQ { get; set; }//IT191
        public decimal URID_DHALL_SQ { get; set; }//IT192
        public decimal AMMA_SALT_RFFIS { get; set; }//IT209
        public decimal AMMA_SALT_DFS { get; set; }//IT210
        public decimal AMMA_SALT_LSS { get; set; }//IT211
        public decimal AMMA_CEMENT { get; set; }//IT212
        public decimal AMMA_SALT_CIS { get; set; }//IT215
        public decimal OMR_HULLING { get; set; }//IT225
        public decimal BOILED_RICE_FORTIFIED { get; set; }//IT229
        public decimal Fortified_Rice_Kernels { get; set; }//IT254
        public decimal FRK_Blended_Rice { get; set; }//IT255
        //public decimal 50KGS_O_N_B	{ get; set; }
        //public decimal 50KGS_S_S	{ get; set; }
        //public decimal 50KGS_S_W_P	{ get; set; }
        //public decimal 50KGS_G_R_M	
        //public decimal 50KGS_U_S
        //public decimal 50KG_POLYTHENE_GUNNY
        //    public decimal 100KGS_S_W_P	{ get; set; }
        //public decimal 100KGS_U_S{ get; set; }

    }
}
