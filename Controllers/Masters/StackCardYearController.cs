using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackCardYearController : ControllerBase
    {
        private object list;

        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetStackCardYear");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}