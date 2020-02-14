using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageTransactionMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(TransactionMasterEntity typeMasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@TRCode", typeMasterEntity.TRCode));
                parameterList.Add(new KeyValuePair<string, string>("@TRName", typeMasterEntity.TRName));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", typeMasterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", typeMasterEntity.Activeflag));
                parameterList.Add(new KeyValuePair<string, string>("@TransType", typeMasterEntity.TransType));
                return manageSQLConnection.InsertData("InsertTransactionMaster", parameterList);
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
                ds = manageSQLConnection.GetDataSetValues("GetTransactionMaster", parameterList);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class TransactionMasterEntity
    {
        public string TRCode { get; set; }
        public string TRName { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
        public string TransType { get; set; }
    }
}