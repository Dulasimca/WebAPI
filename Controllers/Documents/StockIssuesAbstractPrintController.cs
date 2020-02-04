using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageAllReports.Document;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIssuesAbstractPrintController : ControllerBase
    {
        [HttpPost("id")]
        public Tuple<bool,string,DataSet> Post(GatePassCommonEntity gatePassCommon )
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@DocumenteId", gatePassCommon.DocNumber));
            ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsByDate", sqlParameters);
            ManageIssuesAbstractPrint issuesAbstractPrint = new ManageIssuesAbstractPrint();
            bool result= issuesAbstractPrint.GenerateAbstractPrint(ds, gatePassCommon);
            return new Tuple<bool, string,DataSet>(result, result==true ? "Print Generated Successfully" :"Please contact Administrator", ds);
        }
    }
}