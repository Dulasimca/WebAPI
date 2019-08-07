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
            listParameters.Add(new KeyValuePair<string, string>("@Remarks", transactionStatus.remarks));
            listParameters.Add(new KeyValuePair<string, string>("@Flag1", "1"));
            listParameters.Add(new KeyValuePair<string, string>("@userid", transactionStatus.userid));
            listParameters.Add(new KeyValuePair<string, string>("@RoleId", Convert.ToString(transactionStatus.RoleId)));
            return manageSQLConnection.InsertData("InsertTransactionstatus", listParameters);
        }

        [HttpGet("{id}")]
        public string Get(TransactionEntity entity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
            if (entity.Type == 1)
            {
                listParameters.Add(new KeyValuePair<string, string>("@Docdate", entity.Docdate));
                listParameters.Add(new KeyValuePair<string, string>("@Gcode", entity.Gcode));
                listParameters.Add(new KeyValuePair<string, string>("@RoleId", entity.RoleId));
                ds = manageSQLConnection.GetDataSetValues("GetTransactionstatus", listParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                listParameters.Add(new KeyValuePair<string, string>("@Docdate", entity.Docdate));
                listParameters.Add(new KeyValuePair<string, string>("@RCode", entity.RCode));
                listParameters.Add(new KeyValuePair<string, string>("@RoleId", entity.RoleId));
                ds = manageSQLConnection.GetDataSetValues("GetTransactionStatusByDate", listParameters);
                //Manage Transactionstatus
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }
    }
    public class TransactionEntity
    {
       public string Docdate { get; set; }
       public string Gcode { get; set; }
       public string RoleId { get; set; }
       public string RCode { get; set; }
        public int Type { get; set; }

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
        public int RoleId { get; set; }
    }
}