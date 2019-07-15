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

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardMasterController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageDashboard manageDashboard = new ManageDashboard();
            List<object> list = new List<object>();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds= manageSQLConnection.GetDataSetValues("GetMasterData");
                list = manageDashboard.MasterData(ds);
                return JsonConvert.SerializeObject(list);
            }
            finally
            {
                manageDashboard = null;
                list = null;
                ds.Dispose();
             }
        }

    }
}