using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNCSCAPI.ManageAllReports.StockStatement;
using TNCSCAPI.Models.Documents;
using System.Data;

namespace TNCSCAPI.Controllers.Reports.DailyStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFGenerationController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(DocumentStockReceiptList stockReceipt = null)
        {
            //Check valid user details.
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ManageReport manage = new ManageReport();
            List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
            sqlParameters1.Add(new KeyValuePair<string, string>("@UserName", stockReceipt.UserID));
            DataSet ds = new DataSet();
            ds = manageSQLConnection.GetDataSetValues("GetDocumentDownloadUser", sqlParameters1);
            if (manage.CheckDataAvailable(ds))
            {
                ManagePDFGeneration managePDF = new ManagePDFGeneration();
                var result = managePDF.GeneratePDF(stockReceipt);
                if (result.Item1)
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@Doc", stockReceipt.SRNo));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Status", "1"));
                    manageSQLConnection.UpdateValues("UpdateSRDetailStatus", sqlParameters);
                }
                return result;
            }
            else
            {
                return new Tuple<bool, string>(false, "Please contact HO");
            }
        }

        [HttpPut("{id}")]
        public bool Put(SrEntity srEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ManageReport manage = new ManageReport();
            List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
            sqlParameters1.Add(new KeyValuePair<string, string>("@UserName", srEntity.UserId));
            DataSet ds = new DataSet();
            ds = manageSQLConnection.GetDataSetValues("GetDocumentDownloadUser", sqlParameters1);
            if (manage.CheckDataAvailable(ds))
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Doc", srEntity.DocNumber));
                sqlParameters.Add(new KeyValuePair<string, string>("@Status", srEntity.Status.ToString()));
                return manageSQLConnection.UpdateValues("UpdateSRDetailStatus", sqlParameters);
            }
            else
            {
                return false;
            }
        }

    }
    public class SrEntity
    {
        public string DocNumber { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
    }


}