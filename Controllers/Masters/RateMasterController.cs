using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(RateMasterEntity RateMaster)
        {
            //ManageEmployeeDetailsSQL manageEmployee = new ManageEmployeeDetailsSQL();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@RowID", RateMaster.RowID));
            sqlParameters.Add(new KeyValuePair<string, string>("@ScCode", RateMaster.ScCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Allotment", RateMaster.Allotment));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rate", RateMaster.Rate));
            sqlParameters.Add(new KeyValuePair<string, string>("@EffectDate", RateMaster.EffectDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@EndDate", RateMaster.EndDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreatedDate", RateMaster.CreatedDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Remark", RateMaster.Remark));
            sqlParameters.Add(new KeyValuePair<string, string>("@Activeflag", RateMaster.Activeflag));
            sqlParameters.Add(new KeyValuePair<string, string>("@Hsncode", RateMaster.Hsncode));
            return manageSQL.InsertData("InsertRateMaster", sqlParameters);
        }
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetRateMaster");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }


        public class RateMasterEntity
        {
            public string RowID { get; set; }
            public string ScCode { get; set; }
            public string Allotment { get; set; }
            public string Rate { get; set; }
            public string EffectDate { get; set; }
            public string EndDate { get; set; }
            public string CreatedDate { get; set; }
            public string Remark { get; set; }
            public string Activeflag { get; set; }
            public string Hsncode { get; set; }
        }
    }