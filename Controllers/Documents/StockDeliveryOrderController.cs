using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Models.Documents;
using TNCSCAPI.ManageSQL;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockDeliveryOrderController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post (DocumentDeliveryOrderEntity deliveryOrderEntity)
        {
            ManageDeliveryOrder manageDelivery = new ManageDeliveryOrder();
            return manageDelivery.InsertDeliveryOrderEntry(deliveryOrderEntity);
        }

        [HttpGet("{id}")]
        public string Get(string sValue, int Type)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DoDate", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Dono", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderDetailsByDONO", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }
}