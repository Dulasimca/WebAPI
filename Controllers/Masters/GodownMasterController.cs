using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GodownMasterController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ManageGodownData manageGodownData = new ManageGodownData();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetGodownData");
                var result = manageGodownData.GetGodownDatas(ds);
                return JsonConvert.SerializeObject(result);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}