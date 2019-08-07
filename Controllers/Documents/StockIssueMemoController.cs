using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageDocuments;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIssueMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(DocumentStockIssuesEntity documentStockIssuesEntity)
        {
            StockIssueMemo stockIssueMemo = new StockIssueMemo();
            return stockIssueMemo.InsertStockIssueData(documentStockIssuesEntity);
        }

        [HttpGet("{id}")]
        public string Get(string value, int Type,string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SIDate", value));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SINo", value));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsBySINo", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}