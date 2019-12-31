using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRegionMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(RegionMasterEntity MasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@RGCODE", MasterEntity.RGCODE));
                parameterList.Add(new KeyValuePair<string, string>("@RGNAME", MasterEntity.RGNAME));
                parameterList.Add(new KeyValuePair<string, string>("@SessionFlag", MasterEntity.SessionFlag));
                return manageSQLConnection.InsertData("InsertRegionMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class RegionMasterEntity
    {
        public string RGCODE { get; set; }
        public string RGNAME { get; set; }
        public string SessionFlag { get; set; }
    }
}