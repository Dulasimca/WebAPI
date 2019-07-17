using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI
{
    public class GlobalVariable
    {
        //Testing connection
        //public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id = sqladmin; password =sql@svc&ac!72;";
        //public const string ReportPath = "C://LocalRepository//TNCSCUI//dist//GoodsStock//assets//";

        //Live connection
        public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id = sqladmin; password =sql@svc&ac!34;";
        public const string ReportPath = "C://Repos//LiveCode//TNCSCWebSite//assets//";

        public const string StockDORegisterFileName = "DOREG";
        public const string StockTruckMemoRegisterFileName = "TMREG";
        public const string StockReceiptRegisterFileName = "REREG";
        public const string StockIssueRegisterFileName = "ISREG";

    }  
}
