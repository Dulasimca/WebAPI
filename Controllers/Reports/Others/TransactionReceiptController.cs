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
    public class TransactionReceiptController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] CommodityParameter commodity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", commodity.FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", commodity.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodCode", commodity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", commodity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@TRCODE", commodity.TRCode));
            ds = manageSQLConnection.GetDataSetValues("GetTransactionReceipt", sqlParameters);
            TransactionReceipt stockDeliveryOrder = new TransactionReceipt();
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
                Task.Run(() => stockDeliveryOrder.GenerateTransactionReceipt(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}