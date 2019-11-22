using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;


namespace TNCSCAPI.Controllers.GST.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode="0", int Type=2)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                if (Type == 2)
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
                    ds = manageSQLConnection.GetDataSetValues("GetPartyNameDetails", sqlParameters);
                }
                else
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
                    ds = manageSQLConnection.GetDataSetValues("GetPartyNameDetails", sqlParameters);
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}