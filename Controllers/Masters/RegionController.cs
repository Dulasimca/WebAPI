﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = manageSQLConnection.GetDataSetValues("GetRegionData");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}