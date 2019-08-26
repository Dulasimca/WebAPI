using System;
using System.Collections.Generic;
using System.Data;


namespace TNCSCAPI.ManageAllReports.Stack
{
    public class ManageStackCard
    {
        public List<StackCardEntity> ManageStackBalance(DataSet dataSet)
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
                        //check the Issues details.
                        foreach (DataRow ndr in dataRows)
                        {
                            dClosingBalance = dClosingBalance - Convert.ToDecimal(ndr["TOTAL"]);
                            iIssuesBags = iIssuesBags + Convert.ToInt32(ndr["NoPacking"]);
                            dIssuesQuantity = dIssuesQuantity + Convert.ToDecimal(ndr["TOTAL"]);
                            if (ifirst == 0)
                            {
                                stackCard.IssuesBags = Convert.ToString(ndr["NoPacking"]);
                                stackCard.IssuesQuantity = Convert.ToString(ndr["TOTAL"]);
                                stackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                                stackCardEntities.Add(stackCard);
                            }
                            else
                            {
                                StackCardEntity nstackCard = new StackCardEntity();
                                nstackCard.AckDate = Convert.ToString(ndr["Dates"]);
                                nstackCard.IssuesBags = Convert.ToString(ndr["NoPacking"]);
                                nstackCard.IssuesQuantity = Convert.ToString(ndr["TOTAL"]);
                                nstackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                                stackCardEntities.Add(nstackCard);
                            }
                            ifirst = 1;
                        }
                        irow++;
                        if (ifirst == 0)
                        {
                            stackCard.ClosingBalance = Convert.ToString(dClosingBalance);
                            stackCardEntities.Add(stackCard);
                        }
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
}
