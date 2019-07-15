using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageDocuments;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReceiptController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(DocumentStockReceiptList stockReceipt)
        {
            StockReceipt receipt = new StockReceipt();
            return receipt.InsertReceiptData(stockReceipt);
        }

        [HttpGet("{id}")]
        public string Get(string sValue, int Type)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {               
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SRDate", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetSRDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SINo", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetSRDetailsBySINo", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }
}