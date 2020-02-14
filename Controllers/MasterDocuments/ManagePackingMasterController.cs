using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePackingMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(PackingMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Pcode", masterEntity.Pcode));
                parameterList.Add(new KeyValuePair<string, string>("@PName", masterEntity.PName));
                parameterList.Add(new KeyValuePair<string, string>("@PWeight", Convert.ToString(masterEntity.PWeight)));
                parameterList.Add(new KeyValuePair<string, string>("@PBWeight", masterEntity.PBWeight));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                return manageSQLConnection.InsertData("InsertPackingMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
        [HttpGet]
        public string Get(int Type)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            DataSet ds = new DataSet();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
                ds = manageSQLConnection.GetDataSetValues("GetPackingandWeighment", parameterList);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class PackingMasterEntity
    {
        public string Pcode { get; set; }
        public string PName { get; set; }
        public float PWeight { get; set; }
        public string PBWeight { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}