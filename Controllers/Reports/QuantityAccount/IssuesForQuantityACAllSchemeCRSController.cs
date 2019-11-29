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
    public class IssuesForQuantityACAllSchemeCRSController : ControllerBase
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
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", accountEntity.RCode));
            ds = manageSQLConnection.GetDataSetValues("GetIssuesForQuantityACAllSchemeCRS", sqlParameters);
            //Generate the report.
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                ManageQAIssuesAllScheme manageQAReceipt = new ManageQAIssuesAllScheme();
                Task.Run(() => manageQAReceipt.GenerateQAIssues(ds, accountEntity, GlobalVariable.QAIssuesForAllSchemeCRS));
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return JsonConvert.SerializeObject(string.Empty);

        }
    }
}