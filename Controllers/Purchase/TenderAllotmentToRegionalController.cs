using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderAllotmentToRegionalController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(TenderAllotmentToRegionEntity tenderAllotmentToRegEntity = null)
        {
            ManageSQLForTenderAllotmentToRegion manageSQLConnection = new ManageSQLForTenderAllotmentToRegion();
            return manageSQLConnection.InsertTenderAllotmentToRegional(tenderAllotmentToRegEntity);
        }

        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetTenderAllotmentToRegionDetails");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}