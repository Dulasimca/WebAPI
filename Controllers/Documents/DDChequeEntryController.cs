using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class DDChequeEntryController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string,string> Post(DDChequeEntity chequeEntity = null)
        {
            if (chequeEntity.Type == 2)
            {
                ManageDDCheque manageDDCheque = new ManageDDCheque();
                manageDDCheque.GenerateDDCheque(chequeEntity);
                return new Tuple<bool, string, string>(true, "Print Generated !", chequeEntity.ReceiptNo);
            }
            else
            {
                ManageSQLForDDCheque manageSQL = new ManageSQLForDDCheque();
                return manageSQL.InsertDDChequeEntry(chequeEntity);
            }
        }

        [HttpGet("{id}")]
        public string Get(string GCode, string value, int Type)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@godcode", GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@recdate", value));
                ds = manageSQLConnection.GetDataSetValues("GetDDChequeDetailsByDate", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            } else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@vrno", value));
                ds = manageSQLConnection.GetDataSetValues("GetDDChequeDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }
   }
}