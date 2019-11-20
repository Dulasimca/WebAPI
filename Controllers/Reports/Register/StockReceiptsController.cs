using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TNCSCAPI.ManageAllReports;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReceiptsController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] ReportParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", reportParameter.RCode));
            ds = manageSQLConnection.GetDataSetValues("StockReceiptForRegister", sqlParameters);
            StockReceiptRegister stockDeliveryOrder = new StockReceiptRegister();
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
                Task.Run(() => stockDeliveryOrder.GenerateStockReceiptRegister(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
   
}