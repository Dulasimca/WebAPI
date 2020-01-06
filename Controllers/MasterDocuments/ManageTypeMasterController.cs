using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageTypeMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(TypeMasterEntity typeMasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Tycode", typeMasterEntity.Tycode));
                parameterList.Add(new KeyValuePair<string, string>("@Tyname", typeMasterEntity.Tyname));
                parameterList.Add(new KeyValuePair<string, string>("@TYTransaction", typeMasterEntity.TYTransaction));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", typeMasterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", typeMasterEntity.Activeflag));
                //parameterList.Add(new KeyValuePair<string, string>("@TRCode", typeMasterEntity.TRCode));
                return manageSQLConnection.InsertData("InsertTypeMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class TypeMasterEntity
    {
        public string Tycode { get; set; }
        public string Tyname { get; set; }
        public string TYTransaction { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
        public string TRCode { get; set; }
    }
}