using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Others
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityIssueMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] CommodityParameter commodity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", commodity.FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", commodity.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", commodity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", commodity.TRCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", commodity.RCode));
            ds = manageSQLConnection.GetDataSetValues("GetCommodityIssueMemo", sqlParameters);

            CommodityIssueMemo commodityIssueMemo = new CommodityIssueMemo();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = commodity.GCode,
                    FromDate = commodity.FDate,
                    Todate = commodity.ToDate,
                    UserName = commodity.UserName
                };
              
                Task.Run(() => commodityIssueMemo.GenerateCommodityIssueMemoReport(entity)); //Generate the Report
            }

            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }    
}