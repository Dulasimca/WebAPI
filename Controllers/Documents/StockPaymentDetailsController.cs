using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockPaymentDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StockPayement stockPayement)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (stockPayement.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DoDate", stockPayement.DoDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ReceivorCode", stockPayement.ReceivorCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockPayement.GCode));
                ds = manageSQLConnection.GetDataSetValues("GetPaymentDetailsForShop", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DoDate", stockPayement.DoDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ReceivorCode", stockPayement.ReceivorCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockPayement.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@DoNo", stockPayement.DoNo));
                ds = manageSQLConnection.GetDataSetValues("GetPreviousBalance", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }

        }
    }
    public class StockPayement
    {
        public string DoDate { get; set; }
        public string ReceivorCode { get; set; }
        public string GCode { get; set; }
        public int Type { get; set; }
        public string DoNo { get; set; }
    }
}