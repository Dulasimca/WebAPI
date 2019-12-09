using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Models;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.GunnyReport;

namespace TNCSCAPI.Controllers.Reports.GUGR
{
    [Route("api/[controller]")]
    [ApiController]
    public class GUGRController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] GUGRParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", reportParameter.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Type", reportParameter.Type));
            ds = manageSQLConnection.GetDataSetValues("GETGRGU", sqlParameters);
            ManageGUGR manageGUGR = new ManageGUGR();
            ManageReport manageReport = new ManageReport();
            //filter condotions
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = reportParameter.GCode,
                    FromDate = reportParameter.FromDate,
                    Todate = reportParameter.ToDate,
                    UserName = reportParameter.UserName
                };
                // commodityIssueMemo.GenerateCommodityIssueMemoReport(entity);
                Task.Run(() => manageGUGR.GenerateGUGRReport(entity, reportParameter)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}