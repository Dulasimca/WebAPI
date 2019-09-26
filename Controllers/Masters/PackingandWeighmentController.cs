using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackingandWeighmentController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetPackingandWeighment");
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(ds))
                {
                    return JsonConvert.SerializeObject(ds);
                }
                return JsonConvert.SerializeObject(string.Empty);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}