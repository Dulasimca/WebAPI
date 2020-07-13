using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageMenuMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(int RoleId)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", Convert.ToString(RoleId)));
            ds = manageSQLConnection.GetDataSetValues("GetMenuMaster", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPost("{id}")]
        public bool Post(MenuMasterEntity MasterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@MenuId", Convert.ToString(MasterEntity.MenuId)));
                parameterList.Add(new KeyValuePair<string, string>("@Name", MasterEntity.MenuName));
                parameterList.Add(new KeyValuePair<string, string>("@ID", Convert.ToString(MasterEntity.ID)));
                parameterList.Add(new KeyValuePair<string, string>("@URL", MasterEntity.URL));
                parameterList.Add(new KeyValuePair<string, string>("@ParentId", Convert.ToString(MasterEntity.ParentId)));
                parameterList.Add(new KeyValuePair<string, string>("@IsActive", Convert.ToString(MasterEntity.Active)));
                parameterList.Add(new KeyValuePair<string, string>("@RoleId", Convert.ToString(MasterEntity.RoleId)));
                return manageSQLConnection.InsertData("InsertMenuMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class MenuMasterEntity
    {
        public int MenuId { get; set; }
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public int ParentId { get; set; }
        public int RoleId { get; set; }
        public int Active { get; set; }
    }
}