using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.GodownProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class GodownProfilesController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(GodownProfileEntity godownProfile)
        {
            ManageGodownProfileSQL manageGodown = new ManageGodownProfileSQL();
            return manageGodown.InsertGodownProfile(godownProfile);
        }

        [HttpGet("{id}")]
        public string Get(string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetGodownProfile", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }
    public class GodownProfileEntity
    {
        public string RowId { get; set; }
        public string GodownCode { get; set; }
        public string Gname { get; set; }
        public string desig { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string telno { get; set; }
        public string mobno { get; set; }
        public string faxno { get; set; }
    }
}