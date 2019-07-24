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
    public class OpeningBalanceMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(OpeningBalanceEntity openingBalanceEntity)
        {
            OpeningBalanceMaster openingBalance = new OpeningBalanceMaster();
            return openingBalance.InsertOpeningBalanceMaster(openingBalanceEntity);
        }

        [HttpGet("{id}")]
        public string Get(string ObDate, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
                listParameters.Add(new KeyValuePair<string, string>("@ObDate", ObDate));
            listParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetOpeningBalanceMaster", listParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpPut("{id}")]
        public bool Put(string RowId,double WriteOff)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
            listParameters.Add(new KeyValuePair<string, string>("@Rowid", RowId));
            listParameters.Add(new KeyValuePair<string, string>("@WriteOff", WriteOff.ToString()));
           return  manageSQLConnection.UpdateValues("UpdateOpeningBalanceMaster", listParameters);
        }
    }
}