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
    public class ManageVehicleMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(VehicleMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@VHType", masterEntity.VHType));
                parameterList.Add(new KeyValuePair<string, string>("@VHCode", masterEntity.VHCode));
                return manageSQLConnection.InsertData("InsertVehicleMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class VehicleMasterEntity
    {
        public string VHCode { get; set; }
        public string VHType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
    }
}