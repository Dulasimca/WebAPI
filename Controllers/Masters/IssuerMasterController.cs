using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuerMasterController : ControllerBase
    {

         [HttpGet("{id}")]
        public string Get(string GCode, int Type = 0)
        {
            if(Type==2)
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuersMasterAllData", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuerMaster", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
         
        }

        [HttpPut("{id}")]
        public bool Put(IssuerEntity issuerEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", issuerEntity.IssuerCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Activeflag", issuerEntity.Activeflag));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issuerEntity.ACSCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ACSCode", issuerEntity.Godcode));
            return manageSQLConnection.UpdateValues("GetIssuerMaster", sqlParameters);
        }


    }
    public class IssuerEntity
    {
        public string IssuerCode { get; set; }
        public string Activeflag { get; set; }
        public string Godcode { get; set; }
        public string ACSCode { get; set; }
    }
}