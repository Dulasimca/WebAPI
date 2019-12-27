using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Wages
{
    [Route("api/[controller]")]
    [ApiController]
    public class WagesLoadingController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] ReportParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", reportParameter.RCode));
            if (reportParameter.Type == 1)
            {
                ds = manageSQLConnection.GetDataSetValues("LoadMenwagesLoading", sqlParameters);
            }
            else
            {
                ds = manageSQLConnection.GetDataSetValues("LoadMenwagesUnLoading", sqlParameters);
            }
            //StockReceiptRegister stockDeliveryOrder = new StockReceiptRegister();
            //ManageReport manageReport = new ManageReport();
            //if (manageReport.CheckDataAvailable(ds))
            //{
            //    CommonEntity entity = new CommonEntity
            //    {
            //        dataSet = ds,
            //        GCode = reportParameter.GCode,
            //        FromDate = reportParameter.FromDate,
            //        Todate = reportParameter.ToDate,
            //        UserName = reportParameter.UserName
            //    };
            //    Task.Run(() => stockDeliveryOrder.GenerateStockReceiptRegister(entity)); //Generate the Report
            //}
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}