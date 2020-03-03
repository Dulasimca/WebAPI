using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.Audit_Inception
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditInceptionController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(AuditInceptionEntity entity = null)
        {
            ManageSQLForInception manageSQL = new ManageSQLForInception();
            manageSQL.InsertInceptionDetails(entity);
            return new Tuple<bool, string>(true, "Saved Successfully!");
        }

        [HttpGet("{id}")]
        public string Get(string IDate, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@IDate", IDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetInceptionDetails", sqlParameters);
            return JsonConvert.SerializeObject(ds);
        }
    }


    public class AuditInceptionEntity
    {
        public int InceptionID { get; set; }
        public string InceptionTeam { get; set; }
        public string Name { get; set; }
        public DateTime InceptionDate { get; set; }
        public string Designation { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string Remarks { get; set; }
        public List<InceptionList> InceptionData { get; set; }

    }

    public class InceptionList
    {
        public string InceptionItemID { get; set; }
        public string ITCode { get; set; }
        public string StackRowId { get; set; }
        public string TypeCode { get; set; }
        public decimal Quantity { get; set; }
        public string CurrYear { get; set; }
    }
}