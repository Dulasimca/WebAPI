using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(int roleId)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> parameterValues = new KeyValuePair<string, string>();
            ManageMenu manageMenu = new ManageMenu();
            DataSet ds = new DataSet();
            try
            {
                parameterValues = new KeyValuePair<string, string>("@RoleId", Convert.ToString(roleId));
                parameterList.Add(parameterValues);
                ds = manageSQLConnection.GetDataSetValues("GetMenuMaster", parameterList);
                List<Menu> menus = new List<Menu>();
                menus = manageMenu.ConvertDataTableToList(ds.Tables[0]);
                var reult = manageMenu.GetMenuTree(menus, 0);
                return JsonConvert.SerializeObject(reult);
            }
            finally
            {
                ds.Dispose();
                parameterList = null;
                manageMenu = null;
            }
        }
    }
}