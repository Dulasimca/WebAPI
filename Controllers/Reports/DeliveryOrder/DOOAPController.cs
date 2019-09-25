using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOOAPController : ControllerBase
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
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderschemeOAP", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}