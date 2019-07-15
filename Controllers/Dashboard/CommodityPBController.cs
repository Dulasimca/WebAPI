using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityPBController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            ManageStock manageStock = new ManageStock();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetCommodityPB");
                var result = manageStock.GetPhycialBalance(ds);
                return JsonConvert.SerializeObject(result);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}