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
    public class DailyTruckMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DocumentEntity documentEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCODE", documentEntity.RegionCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentEntity.GodownCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@SDATE", documentEntity.DocumentDate.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", documentEntity.RoleId.ToString()));
            ds = manageSQLConnection.GetDataSetValues("GetTruckMemoByDate", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}