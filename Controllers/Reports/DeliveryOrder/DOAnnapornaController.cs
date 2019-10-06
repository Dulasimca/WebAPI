﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.DeliveryOrder;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOAnnapornaController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSchemeWiseEntity SchemeWise)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", SchemeWise.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", SchemeWise.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", SchemeWise.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderschemeAnnapoorna", sqlParameters);
            ManageDOAnnaporna manageDOAnnaporna = new ManageDOAnnaporna();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = SchemeWise.GCode,
                    FromDate = SchemeWise.FromDate,
                    Todate = SchemeWise.ToDate,
                    UserName = SchemeWise.UserID,
                    GName = SchemeWise.GName,
                    RName = SchemeWise.RName
                };
                // manageDOAllScheme.GenerateDOAllSchemeReport(entity);
                Task.Run(() => manageDOAnnaporna.GenerateDOOAnnapornaScheme(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}