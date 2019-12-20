using System;
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
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public string Get(int Type)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                if (Type == 1)
                {
                    ds = manageSQLConnection.GetDataSetValues("GetNotificationPopup");
                    //return JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    ds = manageSQLConnection.GetDataSetValues("GetNotificationsData");
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }


        [HttpPost("{id}")]
        public bool Post(NotificationEntity notification)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            if(notification.Type == 1)
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@ID", notification.ID));
                sqlParameters.Add(new KeyValuePair<string, string>("@Notes", notification.Notes));
                sqlParameters.Add(new KeyValuePair<string, string>("@Reason", notification.Reason));
                sqlParameters.Add(new KeyValuePair<string, string>("@isActive", notification.isActive));
                sqlParameters.Add(new KeyValuePair<string, string>("@ImageName", notification.ImageName));
                return manageSQL.InsertData("InsertNotificationPopup", sqlParameters);
            }
            else
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@ID", notification.ID));
                sqlParameters.Add(new KeyValuePair<string, string>("@Notes", notification.Notes));
                sqlParameters.Add(new KeyValuePair<string, string>("@Reason", notification.Reason));
                sqlParameters.Add(new KeyValuePair<string, string>("@isActive", notification.isActive));
                return manageSQL.InsertData("InsertNotification", sqlParameters);
            }

        }
    }
}

public class NotificationEntity
    {
        public int Type { get; set; }
        public string ID { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
        public string isActive { get; set; }
        public string ImageName { get; set; }
    }