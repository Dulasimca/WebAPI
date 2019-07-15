using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTruckMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(DocumentStockTransferDetails documentStockTransfer)
        {
            ManageTruckMemo manageTruck = new ManageTruckMemo();
            return manageTruck.InsertTruckMemoEntry(documentStockTransfer);
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