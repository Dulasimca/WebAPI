using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandDraftController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] DemandDraftEntity demandDraft)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", demandDraft.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", demandDraft.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", demandDraft.GCode));
            ds = manageSQLConnection.GetDataSetValues("Get_demanddraftdetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        }

    public class DemandDraftEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
      public string UserName { get; set; }
    }
}