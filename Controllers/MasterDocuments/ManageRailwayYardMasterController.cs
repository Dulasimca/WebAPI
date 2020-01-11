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
    public class ManageRailwayYardMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(RailwayYardMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@RYCode", masterEntity.RYCode));
                parameterList.Add(new KeyValuePair<string, string>("@RYName", masterEntity.RYName));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                return manageSQLConnection.InsertData("InsertRailwayYardMaster", parameterList);
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
                ds = manageSQLConnection.GetDataSetValues("GetRailwayYardMaster");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class RailwayYardMasterEntity
    {
        public string RYCode { get; set; }
        public string RYName { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}