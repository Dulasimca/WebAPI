﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderAllotmentDetailsController : ControllerBase
    {

        [HttpPost("{id}")]
        public Tuple<bool, string> Post(TenderAllotmentDetailsEntity tenderAllotmentEntity = null)
        {
            ManageSQLForTenderAllotmentDetails manageSQLConnection = new ManageSQLForTenderAllotmentDetails();
            return manageSQLConnection.InsertTenderAllotmentDetails(tenderAllotmentEntity);
        }

        [HttpGet("{id}")]
        public string Get(string value1, string value2)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", value1));
                    sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", value2));
                    ds = manageSQLConnection.GetDataSetValues("GetTenderAllotmentDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}