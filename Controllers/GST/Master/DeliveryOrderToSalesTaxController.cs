using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.GST.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOrderToSalesTaxController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string fromDate, string toDate , string GCode)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            //sqlParameters.Add(new KeyValuePair<string, string>("@month", Month));
            //sqlParameters.Add(new KeyValuePair<string, string>("@year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@fromDate", fromDate.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@todate", toDate.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetDoGSTSales", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class DOSalesTaxEntity
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
    }
}