using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TNCSCAPI.ManageDocuments;

namespace TNCSCAPI.Controllers.Reports.QuantityAccount
{
    public class ManageQuantityGunnyReceiptandIssues : QuantityGunnyVariables
    {
        ManageReport manageReport = new ManageReport();
        public List<QAGunnyEntity> ProcessQAStatement(QuantityAccountEntity quantityAccountEntity)
        {
            string _itemCode = string.Empty;
            string _ITDescription = string.Empty;

            List<QAGunnyEntity> _qaObjectEntity = new List<QAGunnyEntity>();
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                DataSet dataSetMaster = new DataSet();
                List<KeyValuePair<string, string>> sqlMasterParameters = new List<KeyValuePair<string, string>>();
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@GCode", quantityAccountEntity.GCode));
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@RCode", quantityAccountEntity.RCode));
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@FromDate", quantityAccountEntity.FromDate));

                dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataForGunnyReceiptIssue", sqlMasterParameters);
                DataSet todayIssues = new DataSet();
                DataSet todayReceipt = new DataSet();
                DataSet issuesUptoYesterday = new DataSet();
                DataSet receiptUptoYesterday = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", quantityAccountEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", quantityAccountEntity.RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", quantityAccountEntity.FromDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", quantityAccountEntity.ToDate));
                todayIssues = manageSQLConnection.GetDataSetValues("GetTodayIssuesForQuantityAccount", sqlParameters);
                todayReceipt = manageSQLConnection.GetDataSetValues("GetTodayReceiptForQuantityAccount", sqlParameters);
                issuesUptoYesterday = manageSQLConnection.GetDataSetValues("GetIssuesUptoYesterForQuantityAccount", sqlParameters);
                receiptUptoYesterday = manageSQLConnection.GetDataSetValues("GetReceiptUptoYesterForQuantityAccount", sqlParameters);
                if (dataSetMaster.Tables.Count > 0)
                {
                    foreach (DataRow item in dataSetMaster.Tables[1].Rows) // item master details.
                    {
                        _itemCode = Convert.ToString(item["ITCode"]);
                        _ITDescription = Convert.ToString(item["ITDescription"]);
                        foreach (DataRow godown in dataSetMaster.Tables[2].Rows) // godown details.
                        {
                            QAGunnyEntity objectEntity = new QAGunnyEntity();
                            objectEntity.ItemCode = _itemCode;
                            objectEntity.Commodity = _ITDescription;
                            objectEntity.GCode = godown["TNCSCode"].ToString();
                            objectEntity.RCode = godown["RGCODE"].ToString();
                            objectEntity.GName = godown["TNCSName"].ToString();
                            objectEntity.RName = godown["RGNAME"].ToString();
                            //get opening balance for particualr item.
                            DataRow[] openingBalance = dataSetMaster.Tables[0].Select("GodownCode='" + objectEntity.GCode + "' and CommodityCode='" + _itemCode + "'");

                            DataRow[] receiptuptoYesterday = receiptUptoYesterday.Tables[0].Select("GCode='" + objectEntity.GCode + "' and ITCode='" + _itemCode + "'");
                            // DataRow[] receipttotay = todayReceipt.Tables[0].Select("GCode='" + objectEntity.GCode + "' and ITCode='" + _itemCode + "'");
                            DataRow[] issuesuptoYesterday = issuesUptoYesterday.Tables[0].Select("GCode='" + objectEntity.GCode + "' and ITCode='" + _itemCode + "'");
                            // DataRow[] issuestoday = todayIssues.Tables[0].Select("GCode='" + objectEntity.GCode + "' and ITCode='" + _itemCode + "'");
                            ClearVariable();

                            if (openingBalance != null && openingBalance.Count() > 0)
                            {
                                _BookBalanceWeight = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["BookBalanceWeight"])));
                                //_PhysicalBalanceWeight = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["PhysicalBalanceWeight"])));
                                //_CumulitiveShortage = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["CumulitiveShortage"])));
                                //_Shortage = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(openingBalance[0]["WriteOff"])));
                            }
                            if (receiptuptoYesterday != null && receiptuptoYesterday.Count() > 0)
                            {
                                _receiptuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(receiptuptoYesterday[0]["TOTAL"])));
                            }

                            if (issuesuptoYesterday != null && issuesuptoYesterday.Count() > 0)
                            {
                                _issuesuptoYesterday = Convert.ToDecimal(manageReport.DecimalformatForWeight(Convert.ToString(issuesuptoYesterday[0]["TOTAL"])));
                            }

                            _OpeningBalance = (_BookBalanceWeight + _receiptuptoYesterday) - (_issuesuptoYesterday);

                            _RecPURCHASE = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.PURCHASE.ToString());
                            _RecHULLING = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.HULLING.ToString());
                            _RecGUNNYRELEASE = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.GUNNYRELEASE.ToString());
                            _RecHOPURCHASE = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.HOPURCHASE.ToString());

                            _RecTRANSFERWITHINREGION = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.TRANSFERWITHINREGION.ToString());
                            _RecTRANSFEROTHERREGION = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.TRANSFEROTHERREGION.ToString());
                            _RecEXCESS = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.EXCESS.ToString());

                            //issues details
                            _IsSALES = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.SALES.ToString());
                            _IsGUNNYRELEASE = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.GU.ToString());
                            _IsMENDING = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.MENDING.ToString());
                            _IsTRANSFERWITHINREGION = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.TRANSFERWITHINREGION.ToString());
                            _IsTRANSFEROTHERREGION = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.TRANSFEROTHERREGION.ToString());
                            _IsWRITEOFF = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.WRITEOFF.ToString());


                            _Total = _RecPURCHASE + _RecHOPURCHASE + _RecHULLING + _RecGUNNYRELEASE + _RecEXCESS;
                            objectEntity.OpeningBalance = _OpeningBalance;
                            objectEntity.RecPURCHASE = _RecPURCHASE;
                            objectEntity.RecHULLING = _RecHULLING;
                            objectEntity.RecGUNNYRELEASE = _RecGUNNYRELEASE;
                            objectEntity.RecHOPURCHASE = _RecHOPURCHASE;
                            objectEntity.Total = _Total;
                            objectEntity.RecTRANSFERWITHINREGION = _RecTRANSFERWITHINREGION;
                            objectEntity.RecTRANSFEROTHERREGION = _RecTRANSFEROTHERREGION;
                            objectEntity.RecEXCESS = _RecEXCESS;
                            _TotalReceipt = _Total;
                            objectEntity.TotalReceipt = _TotalReceipt;
                            _GrandTotalReceipt = _TotalReceipt + _OpeningBalance;
                            objectEntity.GrandTotalReceipt = _GrandTotalReceipt;
                            //Issues Details.
                            objectEntity.IsSALES = _IsSALES;
                            objectEntity.IsGUNNYRELEASE = _IsGUNNYRELEASE;
                            objectEntity.IsMENDING = _IsMENDING;
                            _IsTotalSales = _IsSALES + _IsGUNNYRELEASE + _IsMENDING + _IsWRITEOFF;
                            objectEntity.IsTotalSales = _IsTotalSales;
                            objectEntity.IsTRANSFERWITHINREGION = _IsTRANSFERWITHINREGION;
                            objectEntity.IsTRANSFEROTHERREGION = _IsTRANSFEROTHERREGION;
                            objectEntity.IsWRITEOFF = _IsWRITEOFF;
                            _IsTotalIssues = _IsTotalSales;
                            objectEntity.IsTotalIssues = _IsTotalIssues;
                            _IsBalanceQty = _GrandTotalReceipt - _IsTotalIssues;
                            objectEntity.IsBalanceQty = _IsBalanceQty;

                            if (_IsBalanceQty > 0 || _IsTotalSales > 0 ||
                               _GrandTotalReceipt > 0 || _TotalReceipt > 0 || _Total > 0)
                            {
                                _qaObjectEntity.Add(objectEntity);
                            }
                            //decimal CheckData = stockDetailsEntity.OpeningBalance + stockDetailsEntity.ClosingBalance +
                            //               stockDetailsEntity.TotalReceipt + stockDetailsEntity.IssueSales
                            //               + stockDetailsEntity.IssueOthers;
                            //if (CheckData > 0)
                            //{
                            //    dailyStockDetailsEntities.Add(stockDetailsEntity);
                            //}
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" Process Daily Stock " + ex.Message);
            }
            //ManageStockStatement manageStockStatement = new ManageStockStatement();
            //Task.Run(() => manageStockStatement.GenerateStockStatementReport(dailyStockDetailsEntities, stockParameter)); //Generate the Report
            return _qaObjectEntity;
        }


        /// <summary>
        /// Get qty for 
        /// </summary>
        /// <param name="dtQty">Issues or receipt qty</param>
        /// <param name="HeaderData">Datatable for header values</param>
        /// <param name="objectEntity">Entity values</param>
        /// <param name="HeaderType">header values</param>
        /// <returns></returns>
        public decimal GetValue(DataTable dtQty, DataTable dtHeaderData, QAGunnyEntity objectEntity, string HeaderType)
        {
            string SchemeCode = string.Empty, TRCode = string.Empty;
            decimal _qtyData = 0;
            try
            {
                DataRow[] dr = dtHeaderData.Select("HeaderName='" + HeaderType + "'");
                foreach (DataRow ndr in dr)
                {
                    SchemeCode = Convert.ToString(ndr["SCCode"]);
                    TRCode = Convert.ToString(ndr["TRCode"]);
                    // Declare an object variable.
                    object objSum;
                    if (SchemeCode == "All")
                    {
                        objSum = dtQty.Compute("Sum(TOTAL)", "ITCode='" + objectEntity.ItemCode + "' and Trcode='" + TRCode + "' and GCode='" + objectEntity.GCode + "'");
                    }
                    else
                    {
                        objSum = dtQty.Compute("Sum(TOTAL)", "ITCode='" + objectEntity.ItemCode + "' and Scheme ='" + SchemeCode + "' and Trcode='" + TRCode + "' and GCode='" + objectEntity.GCode + "'");
                    }
                    _qtyData = _qtyData + Convert.ToDecimal(manageReport.DecimalformatForWeight(objSum.ToString()));
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            return _qtyData;
        }

    }

    public class QuantityGunnyVariables
    {
        protected void ClearVariable()
        {
            _BookBalanceWeight = 0;
            _OpeningBalance = 0;
            _RecPURCHASE = 0;
            _RecHOPURCHASE = 0;
            _Total = 0;
            _RecHULLING = 0;
            _RecTRANSFERWITHINREGION = 0;
            _RecTRANSFEROTHERREGION = 0;
            _RecEXCESS = 0;
            _RecGUNNYRELEASE = 0;
            _TotalReceipt = 0;
            _GrandTotalReceipt = 0;

            _IsSALES = 0;
            _IsGUNNYRELEASE = 0;
            _IsMENDING = 0;
            _IsTotalSales = 0;
            _IsTRANSFERWITHINREGION = 0;
            _IsTRANSFEROTHERREGION = 0;
            _IsWRITEOFF = 0;
            _IsTotalIssues = 0;
            _IsBalanceQty = 0;

            _receiptuptoYesterday = 0;
            _issuesuptoYesterday = 0;
        }
        protected decimal _issuesuptoYesterday { get; set; }
        protected decimal _receiptuptoYesterday { get; set; }
        protected decimal _BookBalanceWeight { get; set; }
        protected decimal _OpeningBalance { get; set; }
        protected decimal _RecPURCHASE { get; set; }
        protected decimal _RecHOPURCHASE { get; set; }
        protected decimal _Total { get; set; }
        protected decimal _RecHULLING { get; set; }
        protected decimal _RecGUNNYRELEASE { get; set; }
        protected decimal _RecTRANSFERWITHINREGION { get; set; }
        protected decimal _RecTRANSFEROTHERREGION { get; set; }
        protected decimal _RecEXCESS { get; set; }
        protected decimal _TotalReceipt { get; set; }
        protected decimal _GrandTotalReceipt { get; set; }

        protected decimal _IsSALES { get; set; }
        protected decimal _IsGUNNYRELEASE { get; set; }
        protected decimal _IsTotalSales { get; set; }
        protected decimal _IsTRANSFERWITHINREGION { get; set; }
        protected decimal _IsTRANSFEROTHERREGION { get; set; }
        protected decimal _IsWRITEOFF { get; set; }
        protected decimal _IsMENDING { get; set; }
        protected decimal _IsTotalIssues { get; set; }
        protected decimal _IsBalanceQty { get; set; }

    }
    public class QAGunnyEntity
    {
        // Receipt details
        public string ItemCode { get; set; }
        public string Commodity { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal RecPURCHASE { get; set; }
        public decimal RecHOPURCHASE { get; set; }
        public decimal Total { get; set; } // Total receipt except OB
        public decimal RecHULLING { get; set; }
        public decimal RecGUNNYRELEASE { get; set; }
        public decimal RecTRANSFERWITHINREGION { get; set; }
        public decimal RecTRANSFEROTHERREGION { get; set; }
        public decimal RecEXCESS { get; set; }
        public decimal TotalReceipt { get; set; }    // Total Receipt (Total+TotalFreeRice+TotalOtherReceipt)
        public decimal GrandTotalReceipt { get; set; } // OB + TotalReceipt

        // Issues details
        public decimal IsSALES { get; set; }
        public decimal IsGUNNYRELEASE { get; set; }
        public decimal IsTotalSales { get; set; }
        public decimal IsTRANSFERWITHINREGION { get; set; }
        public decimal IsTRANSFEROTHERREGION { get; set; }
        public decimal IsWRITEOFF { get; set; }
        public decimal IsMENDING { get; set; }
        public decimal IsTotalIssues { get; set; }
        public decimal IsBalanceQty { get; set; } //GrandTotalReceipt-TotalIssues
    }
}