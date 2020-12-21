using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockDeliveryOrderController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(DocumentDeliveryOrderEntity deliveryOrderEntity = null)
        {
            if (deliveryOrderEntity.Type == 2)
            {
                ManageDocumentDeliveryOrder documentDO = new ManageDocumentDeliveryOrder();
                documentDO.GenerateDeliveryOrderText(deliveryOrderEntity);
                return new Tuple<bool, string>(true, "Print Generated Successfully");
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
                    //Check data is available in GSTSalesTax table
                    List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                    sqlParameters1.Add(new KeyValuePair<string, string>("@BillNo", deliveryOrderEntity.Dono.Trim()));
                    var result1 = manageSQLConnection.GetDataSetValues("GetSalesTaxByBillNo", sqlParameters1);
                    if (manageReport.CheckDataAvailable(result1))
                    {
                        return new Tuple<bool, string>(false, GlobalVariable.DocumentEditPermissionForDO);
                    }
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
        public string Get(string sValue, int Type, string GCode)
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