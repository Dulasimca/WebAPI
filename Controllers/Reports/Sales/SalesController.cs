using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(IssueMemoEntity issueMemoDetails)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@society", issueMemoDetails.SCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.TCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
            ds = manageSQLConnection.GetDataSetValues("Getissuememo", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpGet("{id}")]
        public string Get(string Fdate, string ToDate, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", Fdate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", GCode)); 
             ds = manageSQLConnection.GetDataSetValues("getIssuememoabstract", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
}

    public class IssueMemoEntity
    {
        public string GCode { get; set; }
        public string SCode { get; set; }
        public string TCode { get; set; }
        public string Fdate { get; set; }
        public string Tdate { get; set; }
    }
}