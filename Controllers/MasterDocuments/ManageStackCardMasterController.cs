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
    public class ManageStackCardMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string StackNo, string CurYear)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@StackNo", StackNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@CurYear", CurYear));
            ds = manageSQLConnection.GetDataSetValues("GetStackCardMaster", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpPost("{id}")]
        public bool Post(StackMasterEntity stackMasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@RowId", stackMasterEntity.RowId));
                parameterList.Add(new KeyValuePair<string, string>("@Flag", stackMasterEntity.Flag));
                return manageSQLConnection.InsertData("UpdateStackCardMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
   public class StackMasterEntity
    {
        public string RowId { get; set; }
        public string Flag { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
        public string StackNo { get; set; }
        public string CurYear { get; set; }
    }
}