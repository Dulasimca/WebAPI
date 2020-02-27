using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.ManageAllReports.StockStatement;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityLedgerController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StockParameter stockParameter)
        {
            ManageCommodityWiseLedger commodityWiseLedger = new ManageCommodityWiseLedger();
            var result = commodityWiseLedger.ProcessCommodityLedger(stockParameter);
            return JsonConvert.SerializeObject(result);
        }
    }
}