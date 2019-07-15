using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackDetailsController : ControllerBase
    {

        [HttpGet("{id}")]
        public string Get(string GCode, string ITCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@CommodityCode", ITCode));
            ds = manageSQLConnection.GetDataSetValues("GetStackDetails", sqlParameters);
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return string.Empty;
        }
    }
}