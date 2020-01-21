using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TNCSCAPI.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDashboardController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageDashboard manageMasterDashboard = new ManageDashboard();
            List<object> list = new List<object>();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetMasterDashboardData");
                list = manageMasterDashboard.MasterDashboardData(ds);
                return JsonConvert.SerializeObject(list);
            }
            finally
            {
                manageMasterDashboard = null;
                list = null;
                ds.Dispose();
            }
        }

    }
}