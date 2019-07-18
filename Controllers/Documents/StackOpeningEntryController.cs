using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.Models.Documents;


namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackOpeningEntryController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(StackOpeningEntity stackOpeningEntity)
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            return manageSQL.InsertStackOpening(stackOpeningEntity);
        }

        [HttpGet("{id}")]
        public string Get(string OBDate, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@ObStackDate", OBDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetStackDetailsByDate", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}