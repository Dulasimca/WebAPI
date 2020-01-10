using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageWeighmentMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(WeighmentMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@WEType", masterEntity.WEType));
                parameterList.Add(new KeyValuePair<string, string>("@WECode", masterEntity.WECode));
                parameterList.Add(new KeyValuePair<string, string>("@Type", masterEntity.Type));
                return manageSQLConnection.InsertData("InsertWeighmentMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetWeighmentMaster");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class WeighmentMasterEntity
    {
        public string Type { get; set; }
        public string WECode { get; set; }
        public string WEType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}