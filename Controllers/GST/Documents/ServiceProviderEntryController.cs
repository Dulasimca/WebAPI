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
    public class ServiceProviderEntryController : ControllerBase
    {
            [HttpPost("{id}")]
            public bool Post(ServiceProviderEntity serviceProvider)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                ManageSQLConnection manageSQL = new ManageSQLConnection();
                sqlParameters.Add(new KeyValuePair<string, string>("@ServiceID", serviceProvider.ServiceID));
                sqlParameters.Add(new KeyValuePair<string, string>("@Month", serviceProvider.Month));
                sqlParameters.Add(new KeyValuePair<string, string>("@Year", serviceProvider.Year));
                sqlParameters.Add(new KeyValuePair<string, string>("@CompanyName", serviceProvider.CompanyName));
                sqlParameters.Add(new KeyValuePair<string, string>("@TIN", serviceProvider.TIN));
                sqlParameters.Add(new KeyValuePair<string, string>("@AccYear", serviceProvider.AccYear));
                sqlParameters.Add(new KeyValuePair<string, string>("@BillNo", serviceProvider.BillNo));
                sqlParameters.Add(new KeyValuePair<string, string>("@BillDate", serviceProvider.BillDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@CommodityName", serviceProvider.CommodityName));
                sqlParameters.Add(new KeyValuePair<string, string>("@TaxType", serviceProvider.TaxType));
                sqlParameters.Add(new KeyValuePair<string, string>("@SGST", serviceProvider.SGST));
                sqlParameters.Add(new KeyValuePair<string, string>("@CGST", serviceProvider.CGST));
                sqlParameters.Add(new KeyValuePair<string, string>("@Amount", serviceProvider.Amount));
                sqlParameters.Add(new KeyValuePair<string, string>("@TaxPercentage", serviceProvider.TaxPercentage));
                sqlParameters.Add(new KeyValuePair<string, string>("@TaxAmount", serviceProvider.TaxAmount));
                sqlParameters.Add(new KeyValuePair<string, string>("@Total", serviceProvider.Total));
                sqlParameters.Add(new KeyValuePair<string, string>("@CreatedBy", serviceProvider.CreatedBy));
                sqlParameters.Add(new KeyValuePair<string, string>("@CreatedDate", serviceProvider.CreatedDate.ToString("MM/dd/yyyy")));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", serviceProvider.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", serviceProvider.RCode));
                return manageSQL.InsertData("InsertServiceProviderEntry", sqlParameters);
            }

            [HttpGet("{id}")]
            public string Get(string RCode, string GCode, string Month, string Year, string AccountingYear)
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
                ds = manageSQLConnection.GetDataSetValues("GetServiceProviderdetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }

        }

        public class ServiceProviderEntity
        {
            public string Month { get; set; }
            public string ServiceID { get; set; }
            public string Year { get; set; }
            public string CompanyName { get; set; }
            public string TIN { get; set; }
            public string BillNo { get; set; }
            public string BillDate { get; set; }
            public string CommodityName { get; set; }
            public string TaxType { get; set; }
            public string CGST { get; set; }
            public string SGST { get; set; }
            public string Amount { get; set; }
            public string TaxPercentage { get; set; }
            public string TaxAmount { get; set; }
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
        }
    }