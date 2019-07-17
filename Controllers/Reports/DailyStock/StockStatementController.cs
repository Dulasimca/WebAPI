using System;
using System.Collections.Generic;
using System.Data;
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
    public class StockStatementController : ControllerBase
    {
       [HttpPost("{id}")]
        public string Post(StockParameter stockParameter)
        {
            StockStatementByDate statementByDate = new StockStatementByDate();
            var result= statementByDate.ProcessStockStatement(stockParameter);
            return JsonConvert.SerializeObject(result);
        }
    }
   
}