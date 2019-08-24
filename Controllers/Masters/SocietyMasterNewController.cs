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
    public class SocietyMasterNewController
    {
        [HttpGet("{id}")]
        public string Get(string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@gcode", GCode));
            ds = manageSQLConnection.GetDataSetValues("[getsocietymasterentry]", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        [HttpPost("{id}")]
        public bool Post(SocietyMasterNewEntity societyMasterNewEntry)
        {
            //ManageEmployeeDetailsSQL manageEmployee = new ManageEmployeeDetailsSQL();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@soccode", societyMasterNewEntry.SocietyCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@socname", societyMasterNewEntry.SocietyName));
            sqlParameters.Add(new KeyValuePair<string, string>("@soctype", societyMasterNewEntry.SocietyType));
            sqlParameters.Add(new KeyValuePair<string, string>("@godowncode", societyMasterNewEntry.gowdoncode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", societyMasterNewEntry.RCode)); 
            sqlParameters.Add(new KeyValuePair<string, string>("@eflag", societyMasterNewEntry.eflag));
            return manageSQL.InsertData("Insertsocietymasterentry", sqlParameters);
        }
    }

    public class SocietyMasterNewEntity
    {
        public string SocietyCode { get; set; }
        public string SocietyName { get; set; }
        public string SocietyType { get; set; }
        public string gowdoncode { get; set; }
        public string RCode{ get; set; }
        public string eflag { get; set; }
    }
}
