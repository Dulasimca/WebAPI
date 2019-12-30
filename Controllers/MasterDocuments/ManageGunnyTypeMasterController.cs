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
    public class ManageGunnyTypeMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(GunnyTypeMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@GTCode", masterEntity.GTCode));
                parameterList.Add(new KeyValuePair<string, string>("@GTType", masterEntity.GTType));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                return manageSQLConnection.InsertData("InsertGunnyTypeMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class GunnyTypeMasterEntity
    {
        public string GTCode { get; set; }
        public string GTType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}