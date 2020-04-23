using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNCSCAPI.ManageAllReports.StockStatement;
using TNCSCAPI.Models.Documents;
using System.Data;
using Newtonsoft.Json;

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

                    List<KeyValuePair<string, string>> InsertParameter = new List<KeyValuePair<string, string>>();
                    InsertParameter.Add(new KeyValuePair<string, string>("@GCode", stockReceipt.ReceivingCode));
                    InsertParameter.Add(new KeyValuePair<string, string>("@RCode", stockReceipt.RCode));
                    InsertParameter.Add(new KeyValuePair<string, string>("@DocNumber", stockReceipt.SRNo));
                    InsertParameter.Add(new KeyValuePair<string, string>("@UserName", stockReceipt.UserID));
                    InsertParameter.Add(new KeyValuePair<string, string>("@DocType", "1"));
                    manageSQLConnection.InsertData("GetDocumentDownloadUser", InsertParameter);
                }
                return result;
            }
            else
            {
                List<KeyValuePair<string, string>> InsertParameter = new List<KeyValuePair<string, string>>();
                InsertParameter.Add(new KeyValuePair<string, string>("@GCode", stockReceipt.ReceivingCode));
                InsertParameter.Add(new KeyValuePair<string, string>("@RCode", stockReceipt.RCode));
                InsertParameter.Add(new KeyValuePair<string, string>("@DocNumber", stockReceipt.SRNo));
                InsertParameter.Add(new KeyValuePair<string, string>("@UserName", stockReceipt.UserID));
                InsertParameter.Add(new KeyValuePair<string, string>("@DocType", "2"));
                manageSQLConnection.InsertData("GetDocumentDownloadUser", InsertParameter);
                //
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

        [HttpGet("{id}")]
        public string Get(DownloadEntity downloadEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
            sqlParameters1.Add(new KeyValuePair<string, string>("@FromDate", downloadEntity.FromDate));
            sqlParameters1.Add(new KeyValuePair<string, string>("@ToDate", downloadEntity.ToDate));
            sqlParameters1.Add(new KeyValuePair<string, string>("@RCode", downloadEntity.RCode));
            sqlParameters1.Add(new KeyValuePair<string, string>("@GCode", downloadEntity.GCode));
            DataSet ds = new DataSet();
            var commandText = (downloadEntity.Type == "1" ? "GetDocumentDownloadLog" : "GetDownloadedReceiptDetails");
            ds = manageSQLConnection.GetDataSetValues(commandText, sqlParameters1);
            return JsonConvert.SerializeObject(ds);

        }

    }
    public class SrEntity
    {
        public string DocNumber { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
    }

    public class DownloadEntity
    {
        public string FromDate { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string ToDate { get; set; }
        public string Type { get; set; }
    }
}