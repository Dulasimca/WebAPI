using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
                parameterList.Add(new KeyValuePair<string, string>("@WECode", Convert.ToString(masterEntity.WECode)));
                return manageSQLConnection.InsertData("InsertWeighmentMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class WeighmentMasterEntity
    {
        public int WECode { get; set; }
        public string WEType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}