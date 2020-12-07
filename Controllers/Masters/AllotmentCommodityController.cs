using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllotmentCommodityController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetAllotmentCommodity");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

        [HttpPost("{id}")]
        public bool Post(AllotmentCommodityEntity allotmentCommodity)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@ACode", allotmentCommodity.ACode));
            sqlParameters.Add(new KeyValuePair<string, string>("@AName", allotmentCommodity.AName));
            sqlParameters.Add(new KeyValuePair<string, string>("@AllotmentCommodity", allotmentCommodity.AllotmentCommodity));
            sqlParameters.Add(new KeyValuePair<string, string>("@AllotmentScheme", allotmentCommodity.AllotmentScheme));
            return manageSQL.InsertData("InsertAllotmentCommodity", sqlParameters);
        }
    }

    public class AllotmentCommodityEntity
    {
        public string ACode { get; set; }
        public string AName { get; set; }
        public string AllotmentCommodity { get; set; }
        public string AllotmentScheme { get; set; }
    }
}