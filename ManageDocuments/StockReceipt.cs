using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageDocuments
{
    public class StockReceipt
    {
        public bool InsertReceiptData(DocumentStockReceiptList stockReceipt)
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            var result = manageSQL.InsertReceiptSrDetailEntry(stockReceipt);
            //if(result!=null)
            //{
            //    string SrNo = result.Item1;
            //    string RowId = result.Item2;
            //    return true;
            //}
            return result;
        }
    }
}
