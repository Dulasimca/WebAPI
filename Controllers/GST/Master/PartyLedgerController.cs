using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.GST.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyLedgerController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(PartyLedgerEntryEntity partyLedger)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@LedgerID", partyLedger.LedgerID));
            sqlParameters.Add(new KeyValuePair<string, string>("@PCode", partyLedger.PCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Pan", partyLedger.Pan));
            sqlParameters.Add(new KeyValuePair<string, string>("@PartyName", partyLedger.PartyName));
            sqlParameters.Add(new KeyValuePair<string, string>("@TIN", partyLedger.Tin));
            sqlParameters.Add(new KeyValuePair<string, string>("@StateCode", partyLedger.StateCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GSTNo", partyLedger.GST));
            sqlParameters.Add(new KeyValuePair<string, string>("@Account", partyLedger.Account));
            sqlParameters.Add(new KeyValuePair<string, string>("@Bank", partyLedger.Bank));
            sqlParameters.Add(new KeyValuePair<string, string>("@Branch", partyLedger.Branch));
            sqlParameters.Add(new KeyValuePair<string, string>("@IFSC", partyLedger.IFSC));
            sqlParameters.Add(new KeyValuePair<string, string>("@Favour", partyLedger.Favour));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", partyLedger.RCode));
            return manageSQL.InsertData("InsertPartyLedgerdetails", sqlParameters);
        }

        [HttpGet("{id}")]
        public string Get(string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            ds = manageSQLConnection.GetDataSetValues("GetPartyLedgerdetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }

    public class PartyLedgerEntryEntity
    {
        public string Pan { get; set; }
        public string PartyName { get; set; }
        public string PCode { get; set; }
        public string GST { get; set; }
        public string Account { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string IFSC { get; set; }
        public string Favour { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string LedgerID { get; set; }
        public string StateCode { get; set; }
        public string Tin { get; set; }
    }
}