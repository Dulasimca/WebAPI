using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Lorry
{
    [Route("api/[controller]")]
    [ApiController]
    public class LorryDetailController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string ToDate, string LorryNo, string DType)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@lorno", LorryNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@todate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@type", DType));
            ds = manageSQLConnection.GetDataSetValues("GetDSlORRYNO", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}