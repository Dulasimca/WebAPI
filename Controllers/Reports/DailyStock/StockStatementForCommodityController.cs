using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.StockStatement;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockStatementForCommodityController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] StockParameterForCommodity stockParameter)
        {          
            StockStatementByCommodity statementByDate = new StockStatementByCommodity();
            var result = statementByDate.ProcessStockStatement(stockParameter);
            return JsonConvert.SerializeObject(result);
        }
    }

    public class StockParameterForCommodity
    {
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}