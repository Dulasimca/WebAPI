using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string TRCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@TRCode", TRCode));
            ds = manageSQLConnection.GetDataSetValues("GetTypeMaster", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}