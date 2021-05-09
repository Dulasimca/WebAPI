using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDStockStatementGodownController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string Date, string Month, string Year, string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
            listParameters.Add(new KeyValuePair<string, string>("@Date", Date));
            listParameters.Add(new KeyValuePair<string, string>("@Year", Year));
            listParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            listParameters.Add(new KeyValuePair<string, string>("@Month", Month));
            ds = manageSQLConnection.GetDataSetValues("GetMasterDataForMDStatementByGodown", listParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}
