using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.CorrectionSlip
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrectionSlipController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] CorrectionEntity correctionEntity )
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Type", correctionEntity.Type));
            sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", correctionEntity.DocNo));
            ds = manageSQLConnection.GetDataSetValues("GetCorrectionSlip", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        
    }

    public class CorrectionEntity
    {
        public string UserID { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string DocNo { get; set; }
        public string Type { get; set; }
    }
}