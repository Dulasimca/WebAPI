using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.Allotment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllotmentBalanceController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(AllotmentBalanceList entity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", entity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", entity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RNCode", entity.RNCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ACSCode", entity.ACSCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", entity.Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", entity.Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@IssRegAdv", entity.IssRegAdv));
            ds = manageSQLConnection.GetDataSetValues("GetAllotmentBalanceDetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpGet("{id}")]
        public string Get(string GCode, string AMonth, string AYear)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@AMonth", AMonth));
            sqlParameters.Add(new KeyValuePair<string, string>("@AYear", AYear));
            ds = manageSQLConnection.GetDataSetValues("GetAllotmentDetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }



    public class AllotmentBalanceList
        {
            public string GCode { get; set; }
            public string RCode { get; set; }
            public string IssRegAdv { get; set; }
            public string Month { get; set; }
            public string RNCode { get; set; }
            public string Year { get; set; }
            public string ACSCode { get; set; }
        }
    }
