using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Documents.TransactionStatus
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionStatusController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(TransactionStatusEntity transactionStatus)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();

            listParameters.Add(new KeyValuePair<string, string>("@srno", transactionStatus.Srno.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@gCode", transactionStatus.Gcode));
            listParameters.Add(new KeyValuePair<string, string>("@docdate", transactionStatus.Docdate.ToString("MM/dd/yyyy")));
            listParameters.Add(new KeyValuePair<string, string>("@receipt", transactionStatus.Receipt.ToString().ToLower() == "false" ? "0" : "1"));
            listParameters.Add(new KeyValuePair<string, string>("@issues", transactionStatus.Issues.ToString().ToLower() == "false" ? "0" : "1"));
            listParameters.Add(new KeyValuePair<string, string>("@transfer", transactionStatus.Transfer.ToString().ToLower() == "false" ? "0" : "1"));
            listParameters.Add(new KeyValuePair<string, string>("@Cb", transactionStatus.CB.ToString().ToLower() == "false" ? "0" : "1"));
            //listParameters.Add(new KeyValuePair<string, string>("@approvaldate", DateTime.Now.ToString()));
            //listParameters.Add(new KeyValuePair<string, string>("@LastUpdated", DateTime.Now.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@Remarks", transactionStatus.remarks));
            listParameters.Add(new KeyValuePair<string, string>("@Flag1", "1"));
            listParameters.Add(new KeyValuePair<string, string>("@userid", transactionStatus.userid));
            return manageSQLConnection.InsertData("InsertTransactionstatus", listParameters);
        }

        [HttpGet("{id}")]
        public string Get(string Docdate, string Gcode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
            listParameters.Add(new KeyValuePair<string, string>("@docdate", Docdate));
            listParameters.Add(new KeyValuePair<string, string>("@gCode", Gcode));
            ds = manageSQLConnection.GetDataSetValues("GetTransactionstatus", listParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class TransactionStatusEntity
    {
        public int Srno { get; set; }
        public string Gcode { get; set; }
        public DateTime Docdate { get; set; }
        public bool Receipt { get; set; }
        public bool Issues { get; set; }
        public bool Transfer { get; set; }
        public bool CB { get; set; }
        public DateTime Approvaldate { get; set; }
        public bool Flag1 { get; set; }
        public DateTime lastupdated { get; set; }
        public string remarks { get; set; }
        public string userid { get; set; }
    }
}