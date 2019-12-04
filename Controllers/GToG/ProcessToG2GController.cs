using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.DataTransfer;

namespace TNCSCAPI.Controllers.G2G
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessToG2GController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post([FromBody]List<DataTransferEntity> entity)
        {
            ManageDataTransfer manageSQLConnection = new ManageDataTransfer();
            foreach (var item in entity)
            {
                 manageSQLConnection.InsertDataTransfer(item);

            }
            return true;
        }

        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Date)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Date", Date));
                ds = manageSQLConnection.GetDataSetValues("GetProcessToG2gData", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}