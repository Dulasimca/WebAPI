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
    public class SchemeWiseController
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSchemeWiseEntity SchemeWise)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", SchemeWise.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", SchemeWise.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", SchemeWise.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@SocCode", SchemeWise.SCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Schcode", SchemeWise.SchCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderschemewise", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class DeliveryOrderSchemeWiseEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string SCode { get; set; }
        public string SchCode { get; set; }
    }
}
