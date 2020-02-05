using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpPost("{id}")]
        public Tuple<bool,string,string> Post(GatePassCommonEntity gatePassCommon)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@DocumentId", gatePassCommon.DocNumber));
            ds = manageSQLConnection.GetDataSetValues("GetStockIssuesForAbstractPrint", sqlParameters);
            ManageIssuesAbstractPrint issuesAbstractPrint = new ManageIssuesAbstractPrint();
            bool result= issuesAbstractPrint.GenerateAbstractPrint(ds, gatePassCommon);
            return new Tuple<bool, string,string>(result, result==true ? "Gate Pass Generated Successfully" :"Please contact Administrator", JsonConvert.SerializeObject(ds.Tables[0]));
        }
    }
}