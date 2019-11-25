using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderOrderNoListController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string value, string Type)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", value));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type));
                    ds = manageSQLConnection.GetDataSetValues("GetTenderOrderNoList", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}