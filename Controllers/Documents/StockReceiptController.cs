﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.ManageDocuments;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReceiptController : ControllerBase
    {
        //[HttpPost("{id}")]
        //public Tuple<bool, string> Post(DocumentStockReceiptList stockReceipt = null)
        //{


        //    ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
        //        List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
        //        sqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockReceipt.ReceivingCode));
        //        var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
        //        ManageReport manageReport = new ManageReport();
        //    if (manageReport.CheckDataAvailable(result))
        //    {
        //        StockReceipt receipt = new StockReceipt();
        //         return receipt.InsertReceiptData(stockReceipt);

        //    }
        //    else
        //    {
        //        return new Tuple<bool, string>(false, "Permission not Granted");
        //    }


        //}

        [HttpPost("{id}")]
        public Tuple<bool, string,string> Post(DocumentStockReceiptList stockReceipt = null)
        {
            ManageDocumentReceipt documentReceipt = new ManageDocumentReceipt();
            if (stockReceipt.Type == 2)
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                  documentReceipt.GenerateReceipt(stockReceipt);
                //update print
                if (stockReceipt.UnLoadingSlip == "N" || string.IsNullOrEmpty(stockReceipt.UnLoadingSlip))
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@SRNo", stockReceipt.SRNo));
                    manageSQLConnection.UpdateValues("UpdateSRDetailsUnLoading", sqlParameters);
                }
                return new Tuple<bool,string,string>(true, "Print Generated Sucessfully","");
            }
            else
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockReceipt.ReceivingCode));
                var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(result))
                {
                    if(stockReceipt.SRNo.Trim() != "0" && stockReceipt.SRNo.Trim() != "-")
                    {
                        List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                        sqlParameters1.Add(new KeyValuePair<string, string>("@Type","1"));
                        sqlParameters1.Add(new KeyValuePair<string, string>("@DocNumber", stockReceipt.SRNo.Trim()));
                        var result1 = manageSQLConnection.GetDataSetValues("CheckDocumentEdit", sqlParameters1);
                        if (!manageReport.CheckDataAvailable(result1))
                        {
                            return new Tuple<bool, string, string>(false,GlobalVariable.DocumentEditPermission, "");
                        }
                        // CheckDocumentEdit
                    }
                    else if(documentReceipt.CheckSRUpdateStatus(stockReceipt.SRNo))
                    {
                        return new Tuple<bool, string, string>(false, GlobalVariable.DocumentEditByHO, "");
                    }
                    StockReceipt receipt = new StockReceipt();
                    return receipt.InsertReceiptData(stockReceipt);
                }
                else
                {
                    return new Tuple<bool, string,string>(false, "Permission not Granted","");
                }
            }
        }

        [HttpGet("{id}")]
        public string Get(string sValue, int Type, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SRDate", sValue));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetSRDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SINo", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetSRDetailsBySINo", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPut("{id}")]
        public bool Put(PrintEntity entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@SRNo", entity.DOCNumber));
            return manageSQLConnection.UpdateValues("UpdateSRDetailsUnLoading", sqlParameters);
        }
    }
    public class PrintEntity
    {
        public string DOCNumber { get; set; }
    }
}