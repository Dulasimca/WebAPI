using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.GST.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTaxEntryController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(PurchaseTaxEntity purchaseTax)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@PurchaseID", purchaseTax.PurchaseID));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", purchaseTax.Month)); 
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", purchaseTax.Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@CompanyName", purchaseTax.CompanyName));
            sqlParameters.Add(new KeyValuePair<string, string>("@AccYear", purchaseTax.AccYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@BillNo", purchaseTax.BillNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@BillDate", purchaseTax.BillDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@CommodityName", purchaseTax.CommodityName));
            sqlParameters.Add(new KeyValuePair<string, string>("@Quantity", purchaseTax.Quantity));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rate", purchaseTax.Rate));
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxType", purchaseTax.TaxType)); 
            sqlParameters.Add(new KeyValuePair<string, string>("@GSTType", purchaseTax.GSTType));
            sqlParameters.Add(new KeyValuePair<string, string>("@Scheme", purchaseTax.Scheme));
            sqlParameters.Add(new KeyValuePair<string, string>("@AADS", purchaseTax.AADS));
            sqlParameters.Add(new KeyValuePair<string, string>("@Measurement", purchaseTax.Measurement));
            sqlParameters.Add(new KeyValuePair<string, string>("@Hsncode", purchaseTax.Hsncode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Amount", purchaseTax.Amount));
            sqlParameters.Add(new KeyValuePair<string, string>("@Percentage", purchaseTax.Percentage));
            sqlParameters.Add(new KeyValuePair<string, string>("@CGST", purchaseTax.CGST));
            sqlParameters.Add(new KeyValuePair<string, string>("@SGST", purchaseTax.SGST));
            sqlParameters.Add(new KeyValuePair<string, string>("@IGST", purchaseTax.IGST));
            sqlParameters.Add(new KeyValuePair<string, string>("@VatAmount", purchaseTax.VatAmount));
            sqlParameters.Add(new KeyValuePair<string, string>("@Total", purchaseTax.Total));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreatedBy", purchaseTax.CreatedBy));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreatedDate", purchaseTax.CreatedDate.ToString("MM/dd/yyyy")));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", purchaseTax.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", purchaseTax.RCode));
            return manageSQL.InsertData("InsertPurchaseTaxEntry", sqlParameters);
        }

        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Month, string Year, string AccountingYear, string GSTType, string TaxPer = null)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            var commandText = (GSTType == "3") ? "GetGSTPurchaseTaxDetails" : "GetPurchaseTaxdetails";
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@AccountingYear", AccountingYear));
            if (GSTType != "3") {
                sqlParameters.Add(new KeyValuePair<string, string>("@GType", GSTType));
            }
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxPer", TaxPer));
            ds = manageSQLConnection.GetDataSetValues(commandText, sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }

    public class PurchaseTaxEntity
    {
        public string Month { get; set; }
        public string PurchaseID { get; set; }
        public string Year { get; set; }
        public string CompanyName { get; set; }
        public string TIN { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string CommodityName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Percentage { get; set; }
        public string VatAmount { get; set; }
        public string Total { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string AccRegion { get; set; }
        public string AccYear { get; set; }
        public string State { get; set; }
        public string GST { get; set; }
        public string Pan { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string RoleId { get; set; }
        public string Measurement { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string GSTType { get; set; }
        public string AADS { get; set; }
        public string Scheme { get; set; }
        public string TaxType { get; set; }
        public string Hsncode { get; set; }
        
    }
}