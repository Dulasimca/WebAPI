using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNCSCAPI.ManageAllReports.StockStatement;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFGenerationController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(DocumentStockReceiptList stockReceipt = null)
        {
            ManagePDFGeneration managePDF = new ManagePDFGeneration();
            var result= managePDF.GeneratePDF(stockReceipt);
            if(result.Item1)
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Doc", stockReceipt.SRNo));
                sqlParameters.Add(new KeyValuePair<string, string>("@Status", "1"));
                 manageSQLConnection.UpdateValues("UpdateSRDetailStatus", sqlParameters);
            }
            return result;
        }

        [HttpPut("{id}")]
        public bool Put(SrEntity srEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Doc", srEntity.DocNumber));
            sqlParameters.Add(new KeyValuePair<string, string>("@Status", srEntity.Status.ToString()));
           return manageSQLConnection.UpdateValues("UpdateSRDetailStatus", sqlParameters);
        }

    }
    public class SrEntity
    {
        public string DocNumber { get; set; }
        public int Status { get; set; }
    }


}