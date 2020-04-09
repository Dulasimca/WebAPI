using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageQuantityAccountController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(QuantityAccountEntity QuantityMaster)
        {
            //ManageEmployeeDetailsSQL manageEmployee = new ManageEmployeeDetailsSQL();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@SlNo", QuantityMaster.SlNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@ScCode", QuantityMaster.SCCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@TRCode", QuantityMaster.TRCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Activeflag", QuantityMaster.Activeflag));
            sqlParameters.Add(new KeyValuePair<string, string>("@RType", QuantityMaster.ReportType));
            return manageSQL.InsertData("InsertQuantityDetails", sqlParameters);
        }
        [HttpGet("{id}")]
        public string Get(string ReportType, string SCCode, string TRCode,string Activeflag, int Type = 0)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            if (Type == 1)
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@ScCode", SCCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@TRCode", TRCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@ReportType", ReportType));
            }
            else
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@ReportType", ReportType));
            }
            ds = manageSQLConnection.GetDataSetValues("GetQuantityAccount", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }

    public class QuantityAccountEntity
    {
        public string SlNo { get; set; }
        public string SCCode { get; set; }
        public string TRCode { get; set; }
        public string Activeflag { get; set; }
        public string ReportType { get; set; }

    }
}