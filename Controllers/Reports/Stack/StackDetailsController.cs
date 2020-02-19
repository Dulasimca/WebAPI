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
    public class StackDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StackDetails stackEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (stackEntity.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Fyear", stackEntity.StackYear));
                ds = manageSQLConnection.GetDataSetValues("GetStackcardDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }     
            else if (stackEntity.Type == 2)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stackEntity.ItemCode));
                ds = manageSQLConnection.GetDataSetValues("GetStackCardByCommodity", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return string.Empty;
        }
    }
    public class StackDetails
    {
        public string GCode { get; set; }
        public string StackYear { get; set; }
        public int Type { get; set; }
        public string ItemCode { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string UserName { get; set; }
    }
}