using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocietyMasterEntryController
    {
        [HttpGet("{id}")]
        public string Get(string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@gcode", GCode));
            ds = manageSQLConnection.GetDataSetValues("get_societymasterentryissuer", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpPut("{id}")]
        public bool Put(SocietyMasterEntryIssuerEntity societyMasterEntry)
        {
           // DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@soccode", societyMasterEntry.SCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@icode", societyMasterEntry.ICode));
            sqlParameters.Add(new KeyValuePair<string, string>("@itype", societyMasterEntry.IType));
            sqlParameters.Add(new KeyValuePair<string, string>("@gcode", societyMasterEntry.GCode));
            return manageSQLConnection.UpdateValues("update_societymasterentry", sqlParameters);
        }
    }

    public class SocietyMasterEntryIssuerEntity
    {
        public string SCode { get; set; }
        public string ICode { get; set; }
        public string IType { get; set; }
        public string GCode { get; set; }
        public string Tyname { get; set; }
        public string Societyname { get; set; }
        public string Issuername { get; set; }
    }
}
