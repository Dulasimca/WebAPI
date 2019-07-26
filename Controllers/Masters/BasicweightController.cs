using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicweightController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ds = manageSQLConnection.GetDataSetValues("GetBasicWeightMaster");
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}