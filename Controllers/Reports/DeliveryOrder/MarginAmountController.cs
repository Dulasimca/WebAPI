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
    public class MarginAmountController
    {
        [HttpPost("{id}")]
        public string Post(MarginAmountEntity marginAmount)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", marginAmount.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", marginAmount.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", marginAmount.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@SocCode", marginAmount.SCode));
             ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderMarginAmount", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class MarginAmountEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string SCode { get; set; }
    }
}