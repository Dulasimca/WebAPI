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
        public Tuple<bool,string> Post(DocumentStockTransferDetails documentStockTransfer)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentStockTransfer.IssuingCode));
            var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(result))
            {
                ManageTruckMemo manageTruck = new ManageTruckMemo();
                return manageTruck.InsertTruckMemoEntry(documentStockTransfer);
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
                sqlParameters.Add(new KeyValuePair<string, string>("@STDate", sValue));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetStockTransferDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@STNo", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetStockTransferDetailsBySTNO", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }
}