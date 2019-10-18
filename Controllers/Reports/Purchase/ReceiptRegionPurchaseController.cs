using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.Purchase;


namespace TNCSCAPI.Controllers.Reports.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptRegionPurchaseController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] ReportParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", reportParameter.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetReceiptRegionPurchase", sqlParameters);
            RoPurchase RoPurchase = new RoPurchase();
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
                Task.Run(() => RoPurchase.GenerateRoPurchase(entity));
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}