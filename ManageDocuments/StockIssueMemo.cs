using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageDocuments
{
    public class StockIssueMemo
    {
        public Tuple<bool,string,string> InsertStockIssueData (DocumentStockIssuesEntity stockIssueList)
        {
           ManageSQLForIssues manageSQLForIssuesConnection = new ManageSQLForIssues();
           var result = manageSQLForIssuesConnection.InsertIssuesEntry(stockIssueList);
            return result;
        }
    }
}
