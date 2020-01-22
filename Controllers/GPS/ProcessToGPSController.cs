using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.GPS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessToGPSController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Date)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Date", Date));
                ds = manageSQLConnection.GetDataSetValues("GetProcessToGPSData", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}