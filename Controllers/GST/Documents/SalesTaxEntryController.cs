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
    public class SalesTaxEntryController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(SalesTaxEntity salesTax)
        {
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            sqlParameters.Add(new KeyValuePair<string, string>("@SalesID", salesTax.SalesID));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", salesTax.Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", salesTax.Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@CompanyName", salesTax.CompanyName));
            sqlParameters.Add(new KeyValuePair<string, string>("@AccYear", salesTax.AccYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@BillNo", salesTax.BillNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@BillDate", salesTax.BillDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@CommodityName", salesTax.CommodityName));
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxType", salesTax.TaxType));
            sqlParameters.Add(new KeyValuePair<string, string>("@Measurement", salesTax.Measurement));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreditSales", salesTax.CreditSales.ToString().ToLower() == "false" ? "0" : "1"));
            sqlParameters.Add(new KeyValuePair<string, string>("@GSTType", salesTax.GSTType));
            sqlParameters.Add(new KeyValuePair<string, string>("@Scheme", salesTax.Scheme));
            sqlParameters.Add(new KeyValuePair<string, string>("@AADS", salesTax.AADS));
            sqlParameters.Add(new KeyValuePair<string, string>("@SGST", salesTax.SGST));
            sqlParameters.Add(new KeyValuePair<string, string>("@CGST", salesTax.CGST));
            sqlParameters.Add(new KeyValuePair<string, string>("@Hsncode", salesTax.Hsncode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Quantity", salesTax.Quantity));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rate", salesTax.Rate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Amount", salesTax.Amount));
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxPercentage", salesTax.TaxPercentage));
            sqlParameters.Add(new KeyValuePair<string, string>("@TaxAmount", salesTax.TaxAmount));
            sqlParameters.Add(new KeyValuePair<string, string>("@Total", salesTax.Total));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreatedBy", salesTax.CreatedBy));
            sqlParameters.Add(new KeyValuePair<string, string>("@CreatedDate", salesTax.CreatedDate.ToString("MM/dd/yyyy")));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", salesTax.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", salesTax.RCode));
            return manageSQL.InsertData("InsertSalesTaxEntry", sqlParameters);
        }

        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Month, string Year, string AccountingYear, string GSTType)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@AccountingYear", AccountingYear));
            sqlParameters.Add(new KeyValuePair<string, string>("@GType", GSTType));
            ds = manageSQLConnection.GetDataSetValues("GetSalesTaxdetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    }

    public class SalesTaxEntity
    {
        public string Month { get; set; }
        public string SalesID { get; set; }
        public string Year { get; set; }
        public string CompanyName { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string CommodityName { get; set; }
        public string CreditSales { get; set; }
        public string Measurement { get; set; }
        public string TaxType { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string Hsncode { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string TaxPercentage { get; set; }
        public string TaxAmount { get; set; }
        public string Total { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string AccYear { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string RoleId { get; set; }
        public string GSTType { get; set; }
        public string Scheme { get; set; }
        public string AADS { get; set; }
    }
}