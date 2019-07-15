using Microsoft.AspNetCore.Mvc;
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
            listParameters.Add(new KeyValuePair<string, string>("@docdate", transactionStatus.Docdate.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@receipt", transactionStatus.Receipt.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@issues", transactionStatus.Issues.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@transfer", transactionStatus.Transfer.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@Cb", transactionStatus.CB.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@approvaldate", transactionStatus.Approvaldate.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@LastUpdated", transactionStatus.lastupdated.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@Remarks", transactionStatus.remarks));
            listParameters.Add(new KeyValuePair<string, string>("@Flag1", transactionStatus.Flag1.ToString()));
            listParameters.Add(new KeyValuePair<string, string>("@userid", transactionStatus.userid));
            return manageSQLConnection.InsertData("GetOpeningBalanceMaster", listParameters);
        }

        //[HttpGet("{id}")]
        //public string Get(string ObDate, string GCode)
        //{
        //    DataSet ds = new DataSet();
        //    ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
        //    List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
        //    listParameters.Add(new KeyValuePair<string, string>("@ObDate", ObDate));
        //    listParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
        //    ds = manageSQLConnection.GetDataSetValues("GetOpeningBalanceMaster", listParameters);

        //    return JsonConvert.SerializeObject(ds.Tables[0]);
        //}
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