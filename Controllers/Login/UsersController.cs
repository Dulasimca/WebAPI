using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace TNCSCAPI.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string userName)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> parameterValues = new KeyValuePair<string, string>();
            try
            {
                parameterValues = new KeyValuePair<string, string>("@UserName", userName);
                parameterList.Add(parameterValues);
                ds = manageSQLConnection.GetDataSetValues("GetUserMaster", parameterList);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
                parameterList = null;
            }
        }
    }
}