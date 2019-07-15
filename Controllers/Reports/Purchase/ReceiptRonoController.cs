using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TNCSCAPI.Models;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptRonoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] PurchaseParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@orderno", reportParameter.OrderNo));
            ds = manageSQLConnection.GetDataSetValues("Getrono", sqlParameters);           
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}