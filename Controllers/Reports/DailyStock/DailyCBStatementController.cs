using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.StockStatement;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyCBStatementController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetDailyCBData");
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }

        [HttpGet("{id}")]
        public string Get(string Date,string RCode,string GCode, string RoleId)
        {
            try
            {
                StockParameter stockParameter = new StockParameter();
                stockParameter.FDate = Date;
                stockParameter.ToDate = Date;
                stockParameter.RCode = RCode;
                stockParameter.GCode = GCode;
                ManageDailyCBStatement manageDailyCB = new ManageDailyCBStatement();
                var result = manageDailyCB.GetDailyCB(stockParameter);
                return JsonConvert.SerializeObject(result);
            }
            finally
            {
            }
        }
    }
}