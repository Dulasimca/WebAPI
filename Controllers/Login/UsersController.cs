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

        [HttpPost("{id}")]
        public bool Post(UserDetails userDetails)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@UserName", userDetails.UserId));
                parameterList.Add(new KeyValuePair<string, string>("@OldPassword", userDetails.OldPassword));
                parameterList.Add(new KeyValuePair<string, string>("@NewPassword", userDetails.NewPassword));
                return manageSQLConnection.UpdateValues("UpdatePassword", parameterList);
                
            }
            finally
            {
                ds.Dispose();
                parameterList = null;
            }
        }
    }

    public class UserDetails
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}