using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Stack
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackBalanceController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StackEntity stackEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stackEntity.ICode));
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stackEntity.StackDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@StacKNo", stackEntity.TStockNo));
            ds = manageSQLConnection.GetDataSetValues("GetStackBalance", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class StackEntity
    {
        public string GCode { get; set; }
        public string ICode { get; set; }
        public string StackDate { get; set; }
        public string TStockNo { get; set; }
    }
}