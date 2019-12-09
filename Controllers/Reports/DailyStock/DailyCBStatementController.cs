using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyCBStatementController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetDailyCBData");
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }

        [HttpGet("{id}")]
        public string Get(string Date,string RCode,string GCode, string RoleId)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Date", Date));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
                ds = manageSQLConnection.GetDataSetValues("GetDailyCBData", sqlParameters);
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}