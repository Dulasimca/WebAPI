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
    public class DOSocietyAbstractController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSocietyAbstractEntity societyAbstract)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", societyAbstract.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", societyAbstract.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", societyAbstract.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrdeSocietyAbstract", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class DeliveryOrderSocietyAbstractEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string UserName { get; set; }
    }
}