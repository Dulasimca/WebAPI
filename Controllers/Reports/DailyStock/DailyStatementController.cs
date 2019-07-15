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
    public class DailyStatementController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string ITCode1,string ITCode2)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            ManageStock manageStock = new ManageStock();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@ITCode1", ITCode1));
                sqlParameters.Add(new KeyValuePair<string, string>("@ITCode2", ITCode2));
                ds = manageSQLConnection.GetDataSetValues("GetDailyStatements", sqlParameters);
                //Convert to list.
                var result = manageStock.GetDailyStockStatments(ds);
                return JsonConvert.SerializeObject(result);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}