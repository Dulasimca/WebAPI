using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        public Tuple<bool,string> Post(DocumentStockReceiptList stockReceipt)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockReceipt.ReceivingCode));
            var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(result))
            {
                StockReceipt receipt = new StockReceipt();
                return receipt.InsertReceiptData(stockReceipt);
            }
            else
            {
                return new Tuple<bool, string>(false, "Permission not Granted");
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
                sqlParameters.Add(new KeyValuePair<string, string>("@SRDate", sValue));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
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
        [HttpPut("{id}")]
        public bool Put(string sValue)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
           
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SRDate", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetSRDetailsByDate", sqlParameters);
                return true;
        }
    }
}