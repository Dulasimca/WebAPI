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
    public class ManagePackingMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(PackingMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@WECode", Convert.ToString(masterEntity.WECode)));
                parameterList.Add(new KeyValuePair<string, string>("@WEType", masterEntity.WEType));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                return manageSQLConnection.InsertData("InsertWeighmentMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class PackingMasterEntity
    {
        public int WECode { get; set; }
        public string WEType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}