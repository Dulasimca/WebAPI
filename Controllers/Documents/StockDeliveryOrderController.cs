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
using TNCSCAPI.ManageAllReports.Document;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockDeliveryOrderController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool,string> Post(DocumentDeliveryOrderEntity deliveryOrderEntity)
        {
            if (deliveryOrderEntity.Type == 2)
            {
                ManageDocumentDeliveryOrder documentDO = new ManageDocumentDeliveryOrder();
                documentDO.GenerateDeliveryOrderText(deliveryOrderEntity);
                return new Tuple<bool, string>(true,"Print Generated Successfully");
            }
            else
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", deliveryOrderEntity.IssuerCode));
                var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(result))
                {
                    ManageDeliveryOrder manageDelivery = new ManageDeliveryOrder();
                    return manageDelivery.InsertDeliveryOrderEntry(deliveryOrderEntity);
                }
                else
                {
                    return new Tuple<bool, string>(false, "Permission not Granted");
                }
            }
           
        }

        [HttpGet("{id}")]
        public string Get(string sValue, int Type,string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DoDate", sValue));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Dono", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderDetailsByDONO", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds);
        }

    }
}