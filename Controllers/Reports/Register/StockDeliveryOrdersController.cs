﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockDeliveryOrdersController : ControllerBase
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
            ds = manageSQLConnection.GetDataSetValues("StockDeliveryOrderForRegister", sqlParameters);
            StockDeliveryOrderRegister stockDeliveryOrder = new StockDeliveryOrderRegister();
            ManageReport manageReport = new ManageReport();
            DataTable dt = new DataTable();
             dt = stockDeliveryOrder.ManageDORegister(ds);
            if (manageReport.CheckDataAvailable(dt))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataTable = dt,
                    GCode = reportParameter.GCode,
                    FromDate = reportParameter.FromDate,
                    Todate = reportParameter.ToDate,
                    UserName = reportParameter.UserName
                };
                Task.Run(() => stockDeliveryOrder.GenerateDeliveryOrderForRegister(entity)); //Generate the Report
            }

            return JsonConvert.SerializeObject(dt);
        }
    }
}