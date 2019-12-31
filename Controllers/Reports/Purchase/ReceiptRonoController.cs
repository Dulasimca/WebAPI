using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TNCSCAPI.Models;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.Purchase;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptRonoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] PurchaseParameter reportParameter)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", reportParameter.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", reportParameter.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", reportParameter.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@orderno", reportParameter.OrderNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", reportParameter.RCode));
            ds = manageSQLConnection.GetDataSetValues("Getrono", sqlParameters);
            RoNoController RoNoPurchase = new RoNoController();
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
                Task.Run(() => RoNoPurchase.GenerateRoNoPurchase(entity));
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}