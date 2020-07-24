using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Lorry
{
    [Route("api/[controller]")]
    [ApiController]
    public class LorryDetailController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(LorryDetailEntity lorryDetail)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if(lorryDetail.DType == "G")
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", lorryDetail.ToDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", lorryDetail.DType));
                ds = manageSQLConnection.GetDataSetValues("GetDSlORRYNOGATEPASS", sqlParameters);
            } else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@lorno", lorryDetail.LorryNo));
                sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", lorryDetail.FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@todate", lorryDetail.ToDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", lorryDetail.DType));
                ds = manageSQLConnection.GetDataSetValues("GetDSlORRYNO", sqlParameters);
            }  
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class LorryDetailEntity
    {
        public string GCode { get; set; }
        public string LorryNo { get; set; }
        public string DType { get; set; }
        public string FDate { get; set; }
        public string ToDate { get; set; }
        public string RCode { get; set; }
    }
}