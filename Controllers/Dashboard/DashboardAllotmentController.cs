using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardAllotmentController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string Month, string Year, string GCode)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            try
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Month", Month));
                sqlParameters.Add(new KeyValuePair<string, string>("@Year", Year));
                ds = manageSQLConnection.GetDataSetValues("GetAllotmetforSociety", sqlParameters);
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}