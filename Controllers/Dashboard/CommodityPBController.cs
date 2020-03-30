using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityPBController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string Code, string Type)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageStock manageStock = new ManageStock();
            try
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type));
                sqlParameters.Add(new KeyValuePair<string, string>("@Code", Code));
                ds = manageSQLConnection.GetDataSetValues("GetCommodityPB", sqlParameters);
                var result = manageStock.GetPhycialBalance(ds);
                return JsonConvert.SerializeObject(result);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}