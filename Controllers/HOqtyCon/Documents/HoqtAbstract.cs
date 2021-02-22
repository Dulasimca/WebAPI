using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.HOqtyCon.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoqtAbstractController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(HoqtAbstract hoqtyabstract)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", hoqtyabstract.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@QtyMonth", hoqtyabstract.qtyMonth)); 
            sqlParameters.Add(new KeyValuePair<string, string>("@QtyYear", hoqtyabstract.qtyYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", hoqtyabstract.ITCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Trcode", hoqtyabstract.Trcode));
            sqlParameters.Add(new KeyValuePair<string, string>("@OB", hoqtyabstract.OB));
            sqlParameters.Add(new KeyValuePair<string, string>("@PurchaseReceipt", hoqtyabstract.PurchaseReceipt));
            sqlParameters.Add(new KeyValuePair<string, string>("@FreeRice", hoqtyabstract.FreeRice));
            sqlParameters.Add(new KeyValuePair<string, string>("@OtherReceipt", hoqtyabstract.OtherReceipt));
            sqlParameters.Add(new KeyValuePair<string, string>("@TotalReceipt", hoqtyabstract.TotalReceipt));
            sqlParameters.Add(new KeyValuePair<string, string>("@Issueonsales", hoqtyabstract.Issueonsales)); 
            sqlParameters.Add(new KeyValuePair<string, string>("@Freeissue", hoqtyabstract.Freeissue));
            sqlParameters.Add(new KeyValuePair<string, string>("@Otherissue", hoqtyabstract.Otherissue));
            sqlParameters.Add(new KeyValuePair<string, string>("@TotalIssues", hoqtyabstract.TotalIssues));
            sqlParameters.Add(new KeyValuePair<string, string>("@CB", hoqtyabstract.CB));
            sqlParameters.Add(new KeyValuePair<string, string>("@CS", hoqtyabstract.CS));
            sqlParameters.Add(new KeyValuePair<string, string>("@ActualBalance", hoqtyabstract.ActualBalance));
            sqlParameters.Add(new KeyValuePair<string, string>("@HOQtyId", hoqtyabstract.HOQtyId));
            return manageSQL.InsertData("InsertHOqtyabstract", sqlParameters);

        }

        [HttpGet("{id}")]
        public string Get(string RCode, string ITCode, string qtyMonth, string qtyYear, string Trcode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            var commandText =  "GetHOqtyabstract";
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", ITCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@qtyMonth", qtyMonth));
            sqlParameters.Add(new KeyValuePair<string, string>("@qtyYear", qtyYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@Trcode", Trcode));
            ds = manageSQLConnection.GetDataSetValues(commandText, sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class HoqtAbstract
    {
        public string RCode { get; set; }
        public string qtyMonth { get; set; }
        public string qtyYear { get; set; }
        public string ITCode { get; set; }
        public string Trcode { get; set; }
        public string OB { get; set; }
        public string PurchaseReceipt { get; set; }
        public string FreeRice { get; set; }
        public string OtherReceipt { get; set; }
        public string TotalReceipt { get; set; }
        public string Issueonsales { get; set; }
        public string Freeissue { get; set; }
        public string Otherissue { get; set; }
        public string TotalIssues { get; set; }
        public string CB { get; set; }
        public string CS { get; set; }
        public string ActualBalance { get; set; }  
        public string RoleId { get; set; }      
        public string HOQtyId { get; set; }
    }
}