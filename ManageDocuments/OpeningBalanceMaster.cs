using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageDocuments
{
    public class OpeningBalanceMaster
    {
        public bool InsertOpeningBalanceMaster (OpeningBalanceEntity openingBalanceList)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            var result = manageSQLConnection.InsertOpeningBalanceMaster(openingBalanceList);
            return result;
        }
    }
}
