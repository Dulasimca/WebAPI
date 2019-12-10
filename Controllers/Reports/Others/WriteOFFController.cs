using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriteOFFController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string ToDate, string GCode,string  UserName,string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            ds = manageSQLConnection.GetDataSetValues("GetWriteOff", sqlParameters);
            WriteOff writeOff = new WriteOff();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = GCode,
                    FromDate = FDate,
                    Todate = ToDate,
                    UserName = UserName
                };
                Task.Run(() => writeOff.GenerateWriteOffReport(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}