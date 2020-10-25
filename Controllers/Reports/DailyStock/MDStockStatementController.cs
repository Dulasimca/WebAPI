using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDStockStatementController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StockEntity  stockEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (stockEntity.DocType == 1)// issues
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Date", stockEntity.Date));
                sqlParameters.Add(new KeyValuePair<string, string>("@Month", stockEntity.Month));
                sqlParameters.Add(new KeyValuePair<string, string>("@Year", stockEntity.Year));
                ds = manageSQLConnection.GetDataSetValues("GetMasterDataForMDStatement", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class StockEntity
    {
        public int DocType { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
    }
}