using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Controllers.Reports.Stack;

namespace TNCSCAPI.ManageAllReports.Stack
{
    public class ManageStackCard
    {
        private string GName { get; set; }
        private string RName { get; set; }
        private string Commodity { get; set; }
        private string TStockNo { get; set; }
        ManageReport report = new ManageReport();

        public List<StackCardEntity> ManageStackBalance(DataSet dataSet, StackEntity stackEntity)
        {
            try
            {
                List<StackCardEntity> stackCardEntities = new List<StackCardEntity>();
                DataTable dtIssues = new DataTable();
                DataTable dtReceipt = new DataTable();
                DateTime fDate = default(DateTime);
                DateTime SDate = default(DateTime);
                if (dataSet.Tables.Count > 1)
                {
                    dtReceipt = dataSet.Tables[1];
                    int rCount = dtReceipt.Rows.Count;
                    int ifirst = 0;
                    decimal dClosingBalance = 0;
                    int iReceiptBags = 0;
                    decimal dReceiptQuantity = 0;
                    int iIssuesBags = 0;
                    decimal dIssuesQuantity = 0;
                    if (rCount == 1)
                    {
                        SDate = Convert.ToDateTime(dtReceipt.Rows[0][0]);
                    }
                    else if (rCount >= 2)
                    {
                        fDate = Convert.ToDateTime(dtReceipt.Rows[0][0]);
                        SDate = Convert.ToDateTime(dtReceipt.Rows[1][0]);
                    }
                    int irow = 1;
                    foreach (DataRow dr in dtReceipt.Rows)
                    {
                        ifirst = 0;
                        StackCardEntity stackCard = new StackCardEntity();
                        stackCard.AckDate = Convert.ToString(dr["Dates"]);
                        stackCard.ReceiptBags = Convert.ToString(dr["NoPacking"]);
                        stackCard.ReceiptQuantity = Convert.ToString(dr["TOTAL"]);
                        dClosingBalance = dClosingBalance + Convert.ToDecimal(Convert.ToString(dr["TOTAL"]));
                        iReceiptBags = iReceiptBags + Convert.ToInt32(dr["NoPacking"]);
                        dReceiptQuantity = dReceiptQuantity + Convert.ToDecimal(Convert.ToString(dr["TOTAL"]));
                        DataRow[] dataRows = null;
                        fDate = Convert.ToDateTime(dr["Dates"]);
                        // filter the datewise data to get the issues.
                        if (rCount == irow)
                        {
                            SDate = Convert.ToDateTime(dr["Dates"]);
                            dataRows = dataSet.Tables[0].Select("Dates >= '" + fDate + "'");
                        }
                        else if (rCount > irow)
                        {
                            SDate = Convert.ToDateTime(dtReceipt.Rows[irow][0]);
                            dataRows = dataSet.Tables[0].Select("Dates >= '" + fDate + "' and dates < '" + SDate + "'");
                        }
                        stackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                        stackCardEntities.Add(stackCard);
                        //check the Issues details.
                        foreach (DataRow ndr in dataRows)
                        {
                            dClosingBalance = dClosingBalance - Convert.ToDecimal(ndr["TOTAL"]);
                            iIssuesBags = iIssuesBags + Convert.ToInt32(ndr["NoPacking"]);
                            dIssuesQuantity = dIssuesQuantity + Convert.ToDecimal(ndr["TOTAL"]);
                            //Check Date Match
                            StackCardEntity nstackCard = new StackCardEntity();
                            nstackCard.AckDate = Convert.ToString(ndr["Dates"]);
                            nstackCard.IssuesBags = Convert.ToString(ndr["NoPacking"]);
                            nstackCard.IssuesQuantity = Convert.ToString(ndr["TOTAL"]);
                            nstackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                            stackCardEntities.Add(nstackCard);
                            //if (ifirst == 0)
                            //{
                            //    stackCard.IssuesBags = Convert.ToString(ndr["NoPacking"]);
                            //    stackCard.IssuesQuantity = Convert.ToString(ndr["TOTAL"]);
                            //    stackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                            //    stackCardEntities.Add(stackCard);
                            //}
                            //else
                            //{

                            //}
                           // ifirst = 1;
                        }
                        irow++;
                        //if (ifirst == 0)
                        //{
                           
                        //}
                    }
                    
                    //Add Total values
                    StackCardEntity TstackCard = new StackCardEntity();
                    TstackCard.AckDate = "Total";
                    TstackCard.ReceiptBags = Convert.ToString(iReceiptBags);
                    TstackCard.ReceiptQuantity = Convert.ToString(dReceiptQuantity);
                    TstackCard.IssuesBags = Convert.ToString(iIssuesBags);
                    TstackCard.IssuesQuantity = Convert.ToString(dIssuesQuantity);
                    TstackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                    stackCardEntities.Add(TstackCard);
                    // Generate Report
                    ManageStackCardPrint manageStackCard = new ManageStackCardPrint();
                   // manageStackCard.GenerateStackCard(stackCardEntities, stackEntity);
                   Task.Run(() => manageStackCard.GenerateStackCard(stackCardEntities, stackEntity));

                }
                return stackCardEntities;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("ManageStackBalance : " + ex.Message);
                return null;
            }
        }
        public bool CheckStackCard(DataSet ds)
        {
            bool isAvailable = false;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    isAvailable = true;
                }
            }
            return isAvailable;
        }

        public List<TransactionStatusEntity> ManageApprovalStatus(DataSet ds, string Date)
        {
            try
            {
                List<TransactionStatusEntity> transactionStatuses = new List<TransactionStatusEntity>();

                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        TransactionStatusEntity statusEntity = new TransactionStatusEntity();
                        statusEntity.GodownName = Convert.ToString(item["GodownName"]);
                        statusEntity.TransactionDate = Date;
                        DataRow[] rows = ds.Tables[0].Select("GCode='" + Convert.ToString(item["GCode"]) + "'");
                        //Check the Region Approved Status
                        DataRow[] rowsRegion = ds.Tables[2].Select("GCode='" + Convert.ToString(item["GCode"]) + "'");
                        if (rowsRegion.Length > 0)
                        {
                            statusEntity.Status = "Approved";
                        }
                        else
                        {
                            statusEntity.Status = "Pending";
                        }
                        if (rows.Length >= 1)
                        {
                            statusEntity.Receipt = rows[0]["Receipt"].ToString();
                            statusEntity.Issues = rows[0]["Issues"].ToString();
                            statusEntity.Transfer = rows[0]["Transfer"].ToString();
                            statusEntity.ClosingBalance = rows[0]["CB"].ToString();
                            statusEntity.ApprovalDate = rows[0]["Approvaldate"].ToString();
                            statusEntity.UserId = rows[0]["UserId"].ToString();
                        }
                        else
                        {
                            statusEntity.Receipt = "Pending";
                            statusEntity.Issues = "Pending";
                            statusEntity.Transfer = "Pending";
                            statusEntity.ClosingBalance = "Pending";
                            statusEntity.ApprovalDate = string.Empty;
                        }

                        transactionStatuses.Add(statusEntity);
                    }
                }
                return transactionStatuses;

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return null;
            }
        }

        public List<StackCardRegisterEntity> ManageStackCardRegister(DataSet dataSet, StackEntity stackEntity)
        {
            try
            {
                ManageReport report = new ManageReport();
                List<StackCardRegisterEntity> stackCardEntities = new List<StackCardRegisterEntity>();
                DataTable dtIssues = new DataTable();
                DataTable dtReceipt = new DataTable();
                DataTable dtObStack = new DataTable();
                DataTable dtWriteOFF = new DataTable();
                //DateTime fDate = default(DateTime);
                //DateTime SDate = default(DateTime);
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                if (dataSet.Tables.Count > 1)
                {
                    dtIssues = dataSet.Tables[0];
                    dtReceipt = dataSet.Tables[1];
                    dtObStack = dataSet.Tables[2];
                    dtWriteOFF = dataSet.Tables[3];
                    string sStackCard = string.Empty;
                    foreach (DataRow item in dtObStack.Rows)
                    {
                        StackCardRegisterEntity stackCardRegister = new StackCardRegisterEntity();
                        sStackCard = Convert.ToString(item["StockNo"]);

                        //Filter particular stack card only
                        DataRow[] dataRowsIssues = dtIssues.Select("StockNo = '" + sStackCard+"'");
                        DataRow[] dataRowsReceipt = dtReceipt.Select("StockNo = '" + sStackCard + "'");
                        DataRow[] dataRowsWriteOff = dtWriteOFF.Select("StockNo = '" + sStackCard + "'");

                        List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                        sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                        sqlParameters.Add(new KeyValuePair<string, string>("@StacKNo", sStackCard));
                        sqlParameters.Add(new KeyValuePair<string, string>("@ShortYear", stackEntity.StackDate));
                        DataSet dsGUGR = manageSQLConnection.GetDataSetValues("GetGUGRbyStackNo", sqlParameters);

                        stackCardRegister.StackCard = sStackCard;
                        stackCardRegister.OpeningBag= Convert.ToString(item["NoPacking"]);
                        stackCardRegister.OpeningQty = Convert.ToString(item["TOTAL"]);

                        if(dsGUGR.Tables.Count>1)
                        {
                            if(dsGUGR.Tables[0].Rows.Count>0) //GU
                            {
                                stackCardRegister.GU = Convert.ToString(dsGUGR.Tables[0].Rows[0]["NoPacking"]);
                            }
                            else
                            {
                                stackCardRegister.GU = "0";
                            }
                            if (dsGUGR.Tables[1].Rows.Count > 0) //GR
                            {
                                stackCardRegister.GR = Convert.ToString(dsGUGR.Tables[1].Rows[0]["NoPacking"]);
                            }
                            else
                            {
                                stackCardRegister.GR = "0";
                            }
                            if (dsGUGR.Tables[2].Rows.Count > 0) //StackDate and Status
                            {
                                stackCardRegister.FromDate = report.FormatDirectDate(Convert.ToString(dsGUGR.Tables[2].Rows[0]["ObStackDate"]));
                                stackCardRegister.ToDate = report.FormatDirectDate(Convert.ToString(dsGUGR.Tables[2].Rows[0]["ClsStackDate"]));
                                stackCardRegister.StackStatus = Convert.ToString(dsGUGR.Tables[2].Rows[0]["Status"]);
                            }
                        }

                        if (dataRowsReceipt != null && dataRowsReceipt.Count() > 0)
                        {
                            stackCardRegister.ReceiptBag = Convert.ToString(dataRowsReceipt[0]["NoPacking"]);
                            stackCardRegister.ReceiptQty = Convert.ToString(dataRowsReceipt[0]["TOTAL"]);
                        }
                        else
                        {
                            stackCardRegister.ReceiptBag = "0";
                            stackCardRegister.ReceiptQty = "0";
                        }

                        if (dataRowsIssues != null && dataRowsIssues.Count() > 0)
                        {
                            stackCardRegister.IssuesBag = Convert.ToString(dataRowsIssues[0]["NoPacking"]);
                            stackCardRegister.IssuesQty = Convert.ToString(dataRowsIssues[0]["TOTAL"]);
                        }
                        else
                        {
                            stackCardRegister.IssuesBag = "0";
                            stackCardRegister.IssuesQty = "0";
                        }

                        if (dataRowsWriteOff != null && dataRowsWriteOff.Count() > 0)
                        {
                            stackCardRegister.WriteOff = Convert.ToString(dataRowsWriteOff[0]["TOTAL"]);
                        }
                        else
                        {
                            stackCardRegister.WriteOff = "0";
                        }

                        //Balance 
                        int totalReceiptBag = Convert.ToInt32(stackCardRegister.OpeningBag) + Convert.ToInt32(stackCardRegister.ReceiptBag) + Convert.ToInt32(stackCardRegister.GU);
                        int totalIssuesBag =   Convert.ToInt32(stackCardRegister.IssuesBag) + Convert.ToInt32(stackCardRegister.GR);

                        decimal totalReceiptQty = Convert.ToDecimal(stackCardRegister.OpeningQty) + Convert.ToDecimal(stackCardRegister.ReceiptQty);
                        decimal totalIssuesQty = Convert.ToDecimal(stackCardRegister.IssuesQty);
                        
                        stackCardRegister.BalanceBag = Convert.ToString(totalReceiptBag- totalIssuesBag);
                        stackCardRegister.BalanceQty = Convert.ToString(totalReceiptQty - totalIssuesQty);
                        stackCardEntities.Add(stackCardRegister);
                    }

                }
                AuditLog.WriteError("Print start");
                StackCardRegisterPrint stackCardRegisterPrint = new StackCardRegisterPrint();
                stackCardRegisterPrint.GenerateStackCardRegister(stackCardEntities, stackEntity);
                AuditLog.WriteError("Print End");
                return stackCardEntities;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("ManageStackBalance : " + ex.Message +" "+ ex.StackTrace);
                return null;
            }
        }

    }

    public class TransactionStatusEntity
    {
        public string GodownName { get; set; }
        public string TransactionDate { get; set; }
        public string Receipt { get; set; }
        public string Issues { get; set; }
        public string Transfer { get; set; }
        public string ClosingBalance { get; set; }
        public string ApprovalDate { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }

    public class StackCardEntity
    {
        public string AckDate { get; set; }
        public string ReceiptBags { get; set; }
        public string ReceiptQuantity { get; set; }
        public string IssuesBags { get; set; }
        public string IssuesQuantity { get; set; }
        public string ClosingBalance { get; set; }
    }
    public class StackCardRegisterEntity
    {
        public string StackCard { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OpeningBag { get; set; }
        public string OpeningQty { get; set; }
        public string ReceiptBag { get; set; }
        public string GU { get; set; }
        public string ReceiptQty { get; set; }
        public string IssuesBag { get; set; }
        public string GR { get; set; }
        public string IssuesQty { get; set; }
        public string BalanceBag { get; set; }
        public string BalanceQty { get; set; }
        public string StackStatus { get; set; }
        public string WriteOff { get; set; }
    }
}
