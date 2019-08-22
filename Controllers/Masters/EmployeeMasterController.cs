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
    public class EmployeeMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(EmployeeDetailsEntity employeeDetails)
        {
            //ManageEmployeeDetailsSQL manageEmployee = new ManageEmployeeDetailsSQL();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@ecode", employeeDetails.Empno));
            sqlParameters.Add(new KeyValuePair<string, string>("@empname", employeeDetails.Empname));
            sqlParameters.Add(new KeyValuePair<string, string>("@designation", employeeDetails.Designation));
            sqlParameters.Add(new KeyValuePair<string, string>("@refno", employeeDetails.Refno));
            sqlParameters.Add(new KeyValuePair<string, string>("@refdate", employeeDetails.Refdate.ToString("MM/dd/yyyy")));
            sqlParameters.Add(new KeyValuePair<string, string>("@jrdate", employeeDetails.Jrdate.ToString("MM/dd/yyyy")));
            sqlParameters.Add(new KeyValuePair<string, string>("@jrtype", employeeDetails.Jrtype));
            sqlParameters.Add(new KeyValuePair<string, string>("@rcode", employeeDetails.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@gcode", employeeDetails.GCode));
            return manageSQL.InsertData("InsertEmployeeDetails", sqlParameters);
        }

        [HttpGet("{id}")]
        public string Get(string GCode, string RCode, string roleId)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Roleid", roleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode ));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            ds = manageSQLConnection.GetDataSetValues("GetEmployeedetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
       
    }

    public class EmployeeDetailsEntity
    {
        public string Empno { get; set; }
        public string Empname { get; set; }
        public string Designation { get; set; }
        public string Refno { get; set; }
        public DateTime Refdate { get; set; }
        public DateTime Jrdate { get; set; }
        public string Jrtype { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
    }
}