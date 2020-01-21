using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSchemesMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(SchemesMasterEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@SCCode", masterEntity.SCCode));
                parameterList.Add(new KeyValuePair<string, string>("@SCName", masterEntity.SCName));
                parameterList.Add(new KeyValuePair<string, string>("@SCType", masterEntity.SCType));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@Activeflag", masterEntity.Activeflag));
                parameterList.Add(new KeyValuePair<string, string>("@AnnavitranTNCSCID",masterEntity.AnnavitranTNCSCID));
                parameterList.Add(new KeyValuePair<string, string>("@AllotmentScheme", masterEntity.AllotmentScheme));
                return manageSQLConnection.InsertData("InsertSchemesMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class SchemesMasterEntity
    {
        public string SCCode { get; set; }
        public string SCName { get; set; }
        public string SCType { get; set; }
        public string DeleteFlag { get; set; }
        public string Activeflag { get; set; }
        public string AnnavitranTNCSCID { get; set; }
        public string AllotmentScheme { get; set; }
    }
}