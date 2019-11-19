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
    public class TenderDataByOrderNoController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string OrderNo, string RCode, string Type, int Spell)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@OrderNumber", OrderNo));
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Spell", Spell.ToString()));
                ds = manageSQLConnection.GetDataSetValues("GetTenderDataByOrderNumber", sqlParameters);
              
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}