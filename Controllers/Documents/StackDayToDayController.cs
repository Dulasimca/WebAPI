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
    public class StackDayToDayController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(StackDayToDayEntity entity = null)
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();

                DataSet ds = new DataSet();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@RowId", entity.RowId));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Remarks", entity.Remarks));
                sqlParameters.Add(new KeyValuePair<string, string>("@Status", entity.Status));
                ds = manageSQL.GetDataSetValues("UpdateStackDetails", sqlParameters);
                return new Tuple<bool, string> (true, JsonConvert.SerializeObject(ds.Tables[0]));
            
        }
    }

    public class StackDayToDayEntity
    {
        public string GCode { get; set; }
        public string RowId { get; set; }
        public string FromDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}