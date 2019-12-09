using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.Transfer;

namespace TNCSCAPI.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckFromRegionController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string ToDate, string GCode,string UserName, string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            ds = manageSQLConnection.GetDataSetValues("GetTruckfromRegion", sqlParameters);
            ManageTruckFromRegion manageTruckToRegion = new ManageTruckFromRegion();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = GCode,
                    FromDate = FDate,
                    Todate = ToDate,
                    UserName = UserName,
                    GName = ds.Tables[0].Rows[0]["ReceiverName"].ToString(),
                    RName = ds.Tables[0].Rows[0]["RGNAME"].ToString()
                };
                Task.Run(() => manageTruckToRegion.GenerateTruckFromRegion(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}