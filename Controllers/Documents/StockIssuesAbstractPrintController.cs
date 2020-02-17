using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
        [HttpGet("{id}")]
        public string Get(string GCode,string DocDate)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@DocDate", DocDate));
            ds = manageSQLConnection.GetDataSetValues("GetGatePassNumber", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPut("{id}")]
        public bool Put(GatePassCommonEntity entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (entity.Type == 1)
            {
                DataSet ds = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                sqlParameters1.Add(new KeyValuePair<string, string>("@DocumentId", entity.DocNumber));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssuesForAbstractPrint", sqlParameters1);
                ManageIssuesAbstractPrint issuesAbstractPrint = new ManageIssuesAbstractPrint();
                Task.Run(() => issuesAbstractPrint.ProcessDataToGPS(ds, entity));
            }
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GatePassId", entity.GatePassNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
            return manageSQLConnection.UpdateValues("UpdateGatePass", sqlParameters);
        }
     }
}