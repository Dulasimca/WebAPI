using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIssueController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] IssuerParameter issuerParameter)
        {
            DataSet ds = new DataSet();
            DataSet dsNew = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", issuerParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issuerParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", issuerParameter.GCode));
            //sqlParameters.Add(new KeyValuePair<string, string>("@Type", "0"));
            sqlParameters.Add(new KeyValuePair<string, string>("@Startindex", Convert.ToString(issuerParameter.StartIndex)));
            sqlParameters.Add(new KeyValuePair<string, string>("@TotalRows", Convert.ToString(issuerParameter.TotalRecord)));
            ds = manageSQLConnection.GetDataSetValues("StockIssueForRegister", sqlParameters);
            if (issuerParameter.Position == 1)
            {
                List<KeyValuePair<string, string>> sqlParametersNew = new List<KeyValuePair<string, string>>();
                sqlParametersNew.Add(new KeyValuePair<string, string>("@FromDate", issuerParameter.FromDate));
                sqlParametersNew.Add(new KeyValuePair<string, string>("@ToDate", issuerParameter.ToDate));
                sqlParametersNew.Add(new KeyValuePair<string, string>("@GodownCode", issuerParameter.GCode));
               // sqlParametersNew.Add(new KeyValuePair<string, string>("@Type", "1"));
                sqlParametersNew.Add(new KeyValuePair<string, string>("@Startindex", Convert.ToString(issuerParameter.StartIndex)));
                sqlParametersNew.Add(new KeyValuePair<string, string>("@TotalRows", Convert.ToString(issuerParameter.TotalRecord)));
                dsNew = manageSQLConnection.GetDataSetValues("StockIssueForRegister", sqlParametersNew);

                StockIssueRegister stockIssues = new StockIssueRegister();
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(ds))
                {
                    CommonEntity entity = new CommonEntity
                    {
                        dataSet = dsNew,
                        GCode = issuerParameter.GCode,
                        FromDate = issuerParameter.FromDate,
                        Todate = issuerParameter.ToDate,
                        UserName = issuerParameter.UserName

                    };
                    //stockIssues.GenerateStockIssuesRegister(entity);
                    Task.Run(() => stockIssues.GenerateStockIssuesRegister(entity)); //Generate the Report
                }
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class IssuerParameter
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public int StartIndex { get; set; }
        public int TotalRecord { get; set; }
        public int Position { get; set; }
        public string UserName { get; set; }
    }
}