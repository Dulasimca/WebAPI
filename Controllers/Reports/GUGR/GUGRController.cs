using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Models;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.GUGR
{
    [Route("api/[controller]")]
    [ApiController]
    public class GUGRController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] GUGRParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Type", reportParameter.Type));
            ds = manageSQLConnection.GetDataSetValues("GETGRGU", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}