using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string UserName)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@UserName", UserName));
            ds = manageSQLConnection.GetDataSetValues("GetUserMaster", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPost("{id}")]
        public bool Post(UserMasterEntity MasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@UserName", MasterEntity.UserName));
                parameterList.Add(new KeyValuePair<string, string>("@Pwd", MasterEntity.Pwd));
                parameterList.Add(new KeyValuePair<string, string>("@EMailId", MasterEntity.EMailId));
                parameterList.Add(new KeyValuePair<string, string>("@EmpId", MasterEntity.EmpId));
                parameterList.Add(new KeyValuePair<string, string>("@PhoneNumber", MasterEntity.PhoneNumber));
                parameterList.Add(new KeyValuePair<string, string>("@RoleId",Convert.ToString(MasterEntity.RoleId)));
                parameterList.Add(new KeyValuePair<string, string>("@Flag", Convert.ToString(MasterEntity.Flag)));
                parameterList.Add(new KeyValuePair<string, string>("@GodownCode", MasterEntity.GodownCode));
                parameterList.Add(new KeyValuePair<string, string>("@Regioncode", MasterEntity.Regioncode));
                return manageSQLConnection.InsertData("InsertUserMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class UserMasterEntity
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string EMailId { get; set; }
        public string EmpId { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int Flag { get; set; }
        public string GodownCode { get; set; }
        public string Regioncode { get; set; }
    }
}