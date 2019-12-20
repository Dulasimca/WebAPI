using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Section
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyStatementController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] DailyStatementEntity entity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", entity.RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", entity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", entity.ITCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", entity.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", entity.ToDate));
            ds = manageSQLConnection.GetDataSetValues("GetSectionDailyStatement", sqlParameters);
            return JsonConvert.SerializeObject(ds);
        }
    }

    public class DailyStatementEntity
    {
        public string RoleId { get; set; }
        public string ITCode { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}