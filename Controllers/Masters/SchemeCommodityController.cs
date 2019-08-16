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
    public class SchemeCommodityController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ds = manageSQLConnection.GetDataSetValues("GetSchemeCommodity");
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}