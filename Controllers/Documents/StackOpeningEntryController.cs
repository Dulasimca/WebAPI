using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.Models.Documents;
using TNCSCAPI.ManageAllReports.Stack;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackOpeningEntryController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool,bool> Post(StackOpeningEntity stackOpeningEntity)
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            ManageStackCard manageStack = new ManageStackCard();

            if (!string.IsNullOrEmpty(stackOpeningEntity.StackNo))
            {
                bool isInserted = false;
                DataSet ds = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@OBDate", Convert.ToString(stackOpeningEntity.ObStackDate)));
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackOpeningEntity.GodownCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@StackNo", stackOpeningEntity.StackNo));
                ds = manageSQL.GetDataSetValues("FetchStackCard", sqlParameters);
                var result = manageStack.CheckStackCard(ds);
                //Check stack nu
                if(!result)
                {
                    isInserted = manageSQL.InsertStackOpening(stackOpeningEntity);
                }
                return new Tuple<bool, bool>(result, isInserted);
            }
            else
            {
                return new Tuple<bool, bool> (false,false);
            }

        }

        [HttpGet("{id}")]
        public string Get(string ICode, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@ICode", ICode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetStackDetailsByDate", sqlParameters);
            return JsonConvert.SerializeObject(ds);
        }

        [HttpPut("{id}")]
        public bool Put(StackCardEntity stackCardEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RowId", stackCardEntity.RowId));
            sqlParameters.Add(new KeyValuePair<string, string>("@ClosedDate", Convert.ToString(stackCardEntity.ClosedDate)));
            return manageSQLConnection.UpdateValues("UpdateStackDetails", sqlParameters);
        }
    }
    public class StackCardEntity
    {
        public string RowId { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}