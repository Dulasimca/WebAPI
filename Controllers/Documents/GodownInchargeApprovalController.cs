using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class GodownInchargeApprovalController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(GodownApprovalEntity entity = null)
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();

            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", entity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ApprovalID", entity.ApprovalID.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@DocNo", entity.DocNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@DocDate", entity.DocDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerName", entity.IssuerName));
            sqlParameters.Add(new KeyValuePair<string, string>("@Flag", entity.Flag.ToString()));
            ds = manageSQL.GetDataSetValues("InsertGodownApprovalDetails", sqlParameters);
            return new Tuple<bool, string>(true, JsonConvert.SerializeObject(ds));

        }
    }

    public class GodownApprovalEntity
    {
        public string GCode { get; set; }
        public string RCode { get; set; }
        public int ApprovalID { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string IssuerName { get; set; }
        public int Flag { get; set; }
    }
}
