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
    public class IssuesForQuantityAccountController : ControllerBase
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
            ds = manageSQLConnection.GetDataSetValues("GetIssuesForQuantityAC", sqlParameters);
            //Generate the report.
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                ManageQAIssues manageQAIssues = new ManageQAIssues();
                Task.Run(() => manageQAIssues.GenerateQAIssues(ds, accountEntity));
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return JsonConvert.SerializeObject(string.Empty);

        }
    }

    public class QuantityAccountEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string UserId { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
    }


}