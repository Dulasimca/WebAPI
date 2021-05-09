using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class CBFromTNDailyController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ds = manageSQLConnection.GetDataForTNDaily("GetDailyStatementForMD");
            return JsonConvert.SerializeObject(ds);
        }
    }
}