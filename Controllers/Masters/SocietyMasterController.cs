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
    public class SocietyMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetSocietyMaster", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}