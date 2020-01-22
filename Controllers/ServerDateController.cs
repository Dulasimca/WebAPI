using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.ServerDate
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerDateController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {

            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetServerDate");
                return JsonConvert.SerializeObject(ds.Tables[0]);
               // sqlCommand.CommandText = "GetServerDate";
               // var date = (sqlCommand.ExecuteScalar()).ToString();

               // return date;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}