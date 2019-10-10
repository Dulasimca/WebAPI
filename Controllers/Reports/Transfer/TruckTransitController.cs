using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Transfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckTransitController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string ToDate, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetTransitdetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}