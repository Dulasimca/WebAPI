using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReceiptController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] DocumentEntity documentEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCODE", documentEntity.RegionCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentEntity.GodownCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", documentEntity.RoleId.ToString()));
            if (documentEntity.Type == 1)
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", documentEntity.ITCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", documentEntity.FromDate.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@TDATE", documentEntity.ToDate.ToString()));
                ds = manageSQLConnection.GetDataSetValues("GetReceiptCommodityWise", sqlParameters);
            } else
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@SDATE", documentEntity.DocumentDate.ToString()));
                ds = manageSQLConnection.GetDataSetValues("GetReceiptByDate", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class DocumentEntity
    {
        public int RoleId { get; set; }
        public string RegionCode { get; set; }
        public string GodownCode { get; set; }
        public string DocumentDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ITCode { get; set; }
        public int Type { get; set; }
    }
}