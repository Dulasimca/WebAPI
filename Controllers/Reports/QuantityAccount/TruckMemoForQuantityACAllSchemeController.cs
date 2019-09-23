using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.QA;

namespace TNCSCAPI.Controllers.Reports.QuantityAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckMemoForQuantityACAllSchemeController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(QuantityAccountEntity accountEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", accountEntity.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", accountEntity.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", accountEntity.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetTruckMemoForQuantityAC", sqlParameters);
            //Generate the report.
            ManageQAReceipt manageQAReceipt = new ManageQAReceipt();
            Task.Run(() => manageQAReceipt.GenerateQAReceipt(ds, accountEntity, GlobalVariable.QAReceiptForAllScheme, "- Truck Memo Abstract -"));
            return JsonConvert.SerializeObject(ds.Tables[0]);

        }
    }
}