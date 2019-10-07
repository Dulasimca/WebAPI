using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.Register;

namespace TNCSCAPI.Controllers.Reports.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class GSTController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool,string> Post([FromBody] ReportParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", reportParameter.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetGSTData", sqlParameters);
            ManageGST manageGST = new ManageGST();
            ManageReport manageReport = new ManageReport();
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
                 return manageGST.GenerateGSTFile(entity); //Generate the Report
            }

            return new Tuple<bool, string>(false, "GST File is not generated");
        }
    }
}