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
        [HttpPost("{id}")]
        public string Post(SocietyEntity entity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (entity.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@SocietyType", entity.ReceivorType));
                ds = manageSQLConnection.GetDataSetValues("GetSocietyMasterByID", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@SocietyType", entity.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@SocietyCode", entity.SocietyCode));
                ds = manageSQLConnection.GetDataSetValues("GetShopName", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class SocietyEntity
    {
        public string GCode { get; set; }
        public string ReceivorType { get; set; }
        public string SocietyCode { get; set; }
        public int Type { get; set; }
    }
}