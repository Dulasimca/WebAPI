using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.GST.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesTaxAbstractController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Month, string Year, string AccountingYear, string GSTType, string TaxPer = null)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            var commandText =  "GetGSTSalesTaxDetailsAbstract" ;
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@AccountingYear", AccountingYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@GType", GSTType));
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxPer", TaxPer));
            ds = manageSQLConnection.GetDataSetValues(commandText, sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
}