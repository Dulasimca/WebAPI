using System.Data;
using System;
using System.Collections.Generic;


namespace TNCSCAPI.ManageAllReports.Stack
{
    public class ManageStackCard
    {
        public List<StackCardEntity> ManageStackBalance(DataSet dataSet)
        {
            List<StackCardEntity> stackCardEntities = new List<StackCardEntity>();
            DataTable dtIssues = new DataTable();
            DataTable dtReceipt = new DataTable();
            DateTime fDate = default(DateTime);
            DateTime SDate = default(DateTime);
            if (dataSet.Tables.Count>3)
            {
                dtReceipt = dataSet.Tables[1];
                //Check Receipt and issues
                int i = 0;
                int rCount = dtReceipt.Rows.Count;
                if(rCount>=2)
                {
                    fDate = Convert.ToDateTime(dtReceipt.Rows[0][0]);
                    SDate = Convert.ToDateTime(dtReceipt.Rows[1][0]);
                }
                foreach (DataRow dr in dtReceipt.Rows)
                {
                    StackCardEntity stackCard = new StackCardEntity();
                    stackCard.AckDate = Convert.ToString(dr["Dates"]);
                    stackCard.ReceiptBags = Convert.ToString(dr["NoPacking"]);
                    stackCard.ReceiptQuantity = Convert.ToString(dr["TOTAL"]);
                }
                dtIssues = dataSet.Tables[0];

                //check Issues
            }

            return null;
        }
        public bool CheckStackCard(DataSet ds)
        {
            bool isAvailable = false;
            if(ds.Tables.Count>0)
            {
                if(ds.Tables[0].Rows.Count>0)
                {
                    isAvailable = true;
                }
            }
            return isAvailable;
        }

       // public 

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
