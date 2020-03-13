using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpGet]
        public string Get(string sValue,int Type=1)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Type == 1)
                {
                    ManageSQLConnection manageSQL = new ManageSQLConnection();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@TNCSCKey", sValue));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", "1"));
                    ds = manageSQL.GetDataSetValues("GetTNCSCSettings", sqlParameters);
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {

                    ManageSQLConnection manageSQL = new ManageSQLConnection();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@TNCSCKey", sValue));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
                    ds = manageSQL.GetDataSetValues("GetTNCSCSettings", sqlParameters);
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}