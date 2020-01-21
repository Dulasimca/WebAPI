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