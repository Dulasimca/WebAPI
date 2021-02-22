using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.HOqtyCon.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoqtAbstractrepController : ControllerBase
    {
      
        [HttpGet("{id}")]
        public string Get(string RCode, string ITCode, string qtyMonth, string qtyYear, string Trcode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            var commandText = "GetHOqtyabstractrep";
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", ITCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@qtyMonth", qtyMonth));
            sqlParameters.Add(new KeyValuePair<string, string>("@qtyYear", qtyYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@Trcode", Trcode));
            ds = manageSQLConnection.GetDataSetValues(commandText, sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

   
}