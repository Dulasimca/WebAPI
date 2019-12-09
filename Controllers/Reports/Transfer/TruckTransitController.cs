using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.Transfer;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Transfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckTransitController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string ToDate, string GCode, string Username, string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            ds = manageSQLConnection.GetDataSetValues("GetTransitdetails", sqlParameters);
            ManageTruckTransit manageTruckToRegion = new ManageTruckTransit();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = GCode,
                    FromDate = FDate,
                    Todate = ToDate,
                    UserName = Username,
                    GName = ds.Tables[0].Rows[0]["TNCSName"].ToString(),
                    RName = ds.Tables[0].Rows[0]["Region"].ToString()
                };
                Task.Run(() => manageTruckToRegion.GenerateTruckTransit(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}