using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TNCSCAPI.Controllers.Reports.QuantityAccount;
using TNCSCAPI.ManageDocuments;

namespace TNCSCAPI.ManageAllReports.QA
{
    public class ManageQuantityAccountReceiptandIssuesold : QuantityAccountVariables
    {
        ManageReport manageReport = new ManageReport();
        public List<QAObjectEntity> ProcessQAStatement(QuantityAccountEntity quantityAccountEntity)
        {
            string _itemCode = string.Empty;
            string _ITDescription = string.Empty;

            List<QAObjectEntity> _qaObjectEntity = new List<QAObjectEntity>();
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                DataSet dataSetMaster = new DataSet();
                List<KeyValuePair<string, string>> sqlMasterParameters = new List<KeyValuePair<string, string>>();
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@GCode", quantityAccountEntity.GCode));
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@RCode", quantityAccountEntity.RCode));
                sqlMasterParameters.Add(new KeyValuePair<string, string>("@FromDate", quantityAccountEntity.FromDate));

                dataSetMaster = manageSQLConnection.GetDataSetValues("GetMasterDataForQuantityAccountCB", sqlMasterParameters);
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
                            QAObjectEntity objectEntity = new QAObjectEntity();
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

                            _RecPDS = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.PDS.ToString());
                            _RecPRIORITY = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.PRIORITY.ToString());
                            _RecTIDEOVER = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.TIDEOVER.ToString());
                            _RecAAY = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                                EnumQAReceiptParameter.AAY.ToString());
                            _RecSPLPDS = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.SPLPDS.ToString());
                            _RecCEMENT = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.CEMENT.ToString());
                            _RecHOPURCHASE = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.HOPURCHASE.ToString());
                            _RecSEIZUR = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.SEIZUR.ToString());

                            _RecPTMGRNMP = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.PTMGRNMP.ToString());
                            _RecSGRY = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.SGRY.ToString());
                            _RecANNAPURNA = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.ANNAPURNA.ToString());

                            _RecRECEIVEDFROM = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.RECEIVEDFROM.ToString());
                            _RecTRANSFERWITHINREGION = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.TRANSFERWITHINREGION.ToString());
                            _RecTRANSFEROTHERREGION = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.TRANSFEROTHERREGION.ToString());
                            _RecEXCESS = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                        EnumQAReceiptParameter.EXCESS.ToString());
                            _RecCLEANINGANDPACKING = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.CLEANINGANDPACKING.ToString());
                            _RecVCFLOOD = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.VCFLOOD.ToString());
                            _RecSALESRETURN = GetValue(todayReceipt.Tables[0], dataSetMaster.Tables[3], objectEntity,
                                            EnumQAReceiptParameter.SALESRETURN.ToString());

                            //issues details
                            _IsPDS = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.PDS.ToString());
                            _IsCOOP = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.COOP.ToString());
                            _IsPOLICE = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.POLICE.ToString());
                            _IsNMP = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.NMP.ToString());
                            _IsBULK = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.BULK.ToString());
                            _IsCREDIT = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.CREDIT.ToString());
                            _IsOAP = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.OAP.ToString());
                            _IsSRILANKA = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.SRILANKA.ToString());
                            _IsAAY = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.AAY.ToString());
                            _IsSPLPDS = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.SPLPDS.ToString());
                            _IsPDSCOOP = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.PDSCOOP.ToString());
                            _IsCEMENTFLOOD = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.CEMENTFLOOD.ToString());

                            _IsPTMGR = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.PTMGR.ToString());
                            _IsSGRY = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.SGRY.ToString());
                            _IsANNAPOORNA = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.ANNAPOORNA.ToString());

                            _IsISSUESTOPROCESSING = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.ISSUESTOPROCESSING.ToString());
                            _IsTRANSFERWITHINREGION = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.TRANSFERWITHINREGION.ToString());
                            _IsTRANSFEROTHERREGION = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.TRANSFEROTHERREGION.ToString());
                            _IsWRITEOFF = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.WRITEOFF.ToString());
                            _IsCLEANING = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.CLEANING.ToString());
                            _IsVCBLG = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.VCBLG.ToString());
                            _IsPURCHASERETURN = GetValue(todayIssues.Tables[0], dataSetMaster.Tables[4], objectEntity,
                                            EnumQAIssuesHeaderParameter.PURCHASERETURN.ToString());


                            _Total = _RecPDS + _RecPRIORITY + _RecTIDEOVER + _RecAAY + _RecSPLPDS
                                + _RecCEMENT + _RecHOPURCHASE + _RecSEIZUR;
                            objectEntity.OpeningBalance = _OpeningBalance;
                            objectEntity.RecPDS = _RecPDS;
                            objectEntity.RecPRIORITY = _RecPRIORITY;
                            objectEntity.RecTIDEOVER = _RecTIDEOVER;
                            objectEntity.RecAAY = _RecAAY;
                            objectEntity.RecSPLPDS = _RecSPLPDS;
                            objectEntity.RecCEMENT = _RecCEMENT;
                            objectEntity.RecHOPURCHASE = _RecHOPURCHASE;
                            objectEntity.RecSEIZUR = _RecSEIZUR;
                            objectEntity.Total = _Total;
                            _TotalFreeRice = _RecPTMGRNMP + _RecSGRY + _RecANNAPURNA;
                            objectEntity.RecPTMGRNMP = _RecPTMGRNMP;
                            objectEntity.RecSGRY = _RecSGRY;
                            objectEntity.RecANNAPURNA = _RecANNAPURNA;
                            objectEntity.TotalFreeRice = _TotalFreeRice;
                            objectEntity.RecRECEIVEDFROM = _RecRECEIVEDFROM;
                            objectEntity.RecTRANSFERWITHINREGION = _RecTRANSFERWITHINREGION;
                            objectEntity.RecTRANSFEROTHERREGION = _RecTRANSFEROTHERREGION;
                            objectEntity.RecEXCESS = _RecEXCESS;
                            objectEntity.RecCLEANINGANDPACKING = _RecCLEANINGANDPACKING;
                            objectEntity.RecVCFLOOD = _RecVCFLOOD;
                            objectEntity.RecSALESRETURN = _RecSALESRETURN;
                            _TotalOtherReceipt = _RecRECEIVEDFROM + _RecTRANSFERWITHINREGION + _RecTRANSFEROTHERREGION
                                + _RecEXCESS + _RecCLEANINGANDPACKING + _RecSALESRETURN;
                            objectEntity.TotalOtherReceipt = _TotalOtherReceipt;
                            _TotalReceipt = _Total + _TotalFreeRice + _TotalOtherReceipt;
                            objectEntity.TotalReceipt = _TotalReceipt;
                            _GrandTotalReceipt = _TotalReceipt + _OpeningBalance;
                            objectEntity.GrandTotalReceipt = _GrandTotalReceipt;
                            //Issues Details.
                            objectEntity.IsPDS = _IsPDS;
                            objectEntity.IsCOOP = _IsCOOP;
                            objectEntity.IsPOLICE = _IsPOLICE;
                            objectEntity.IsNMP = _IsNMP;
                            objectEntity.IsBULK = _IsBULK;
                            objectEntity.IsCREDIT = _IsCREDIT;
                            objectEntity.IsOAP = _IsOAP;
                            objectEntity.IsSRILANKA = _IsSRILANKA;
                            objectEntity.IsAAY = _IsAAY;
                            objectEntity.IsSPLPDS = _IsSPLPDS;
                            objectEntity.IsPDSCOOP = _IsPDSCOOP;
                            objectEntity.IsCEMENTFLOOD = _IsCEMENTFLOOD;
                            _IsTotalSales = _IsPDS + _IsCOOP + _IsPOLICE + _IsNMP + _IsBULK + _IsCREDIT
                                + _IsOAP + _IsSRILANKA + _IsAAY + _IsSPLPDS + _IsPDSCOOP + _IsCEMENTFLOOD;
                            objectEntity.IsTotalSales = _IsTotalSales;
                            objectEntity.IsPTMGR = _IsPTMGR;
                            objectEntity.IsSGRY = _IsSGRY;
                            objectEntity.IsANNAPOORNA = _IsANNAPOORNA;
                            _IsTotalFreeRiceIssues = _IsPTMGR + _IsSGRY + _IsANNAPOORNA;
                            objectEntity.IsTotalFreeRiceIssues = _IsTotalFreeRiceIssues;
                            objectEntity.IsISSUESTOPROCESSING = _IsISSUESTOPROCESSING;
                            objectEntity.IsTRANSFERWITHINREGION = _IsTRANSFERWITHINREGION;
                            objectEntity.IsTRANSFEROTHERREGION = _IsTRANSFEROTHERREGION;
                            objectEntity.IsWRITEOFF = _IsWRITEOFF;
                            objectEntity.IsCLEANING = _IsCLEANING;
                            objectEntity.IsVCBLG = _IsVCBLG;
                            objectEntity.IsPURCHASERETURN = _IsPURCHASERETURN;
                            _IsTotalOtherIssues = _IsISSUESTOPROCESSING + _IsTRANSFERWITHINREGION + _IsTRANSFEROTHERREGION
                                + _IsWRITEOFF + _IsCLEANING + _IsVCBLG + _IsPURCHASERETURN;
                            objectEntity.IsTotalOtherIssues = _IsTotalOtherIssues;
                            _IsTotalIssues = _IsTotalSales + _IsTotalFreeRiceIssues + _IsTotalOtherIssues;
                            objectEntity.IsTotalIssues = _IsTotalIssues;
                            _IsBalanceQty = _GrandTotalReceipt - _IsTotalIssues;
                            objectEntity.IsBalanceQty = _IsBalanceQty;

                            if (_IsBalanceQty > 0 || _IsTotalOtherIssues > 0 || _IsTotalFreeRiceIssues > 0 || _IsTotalSales > 0 ||
                               _GrandTotalReceipt > 0 || _TotalReceipt > 0 || _TotalOtherReceipt > 0 || _Total > 0 || _TotalFreeRice > 0)
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
        public decimal GetValue(DataTable dtQty, DataTable dtHeaderData, QAObjectEntity objectEntity, string HeaderType)
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
}