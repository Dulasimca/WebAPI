using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.GST.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class EInvoiceController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(EInvoiceEntity einvoice)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (einvoice.DType == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DType", einvoice.DType.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@DONumber", einvoice.DONumber));
                ds = manageSQLConnection.GetDataSetValues("GetEInvoice", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", einvoice.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", einvoice.RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", einvoice.fromdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@todate", einvoice.todate));
                ds = manageSQLConnection.GetDataSetValues("GetEInvoice", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class EInvoiceEntity
    {
        public int DType { get; set; }
        public string DONumber { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}