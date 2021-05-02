using Microsoft.AspNetCore.Mvc;
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
    public class StockIssueMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string, string> Post(DocumentStockIssuesEntity documentStockIssuesEntity = null)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ManageReport manageReport = new ManageReport();
            //Check the document Approval
            List<KeyValuePair<string, string>> sqlParametersDocApproval = new List<KeyValuePair<string, string>>();
            sqlParametersDocApproval.Add(new KeyValuePair<string, string>("@DocDate", documentStockIssuesEntity.SIDate));
            sqlParametersDocApproval.Add(new KeyValuePair<string, string>("@GCode", documentStockIssuesEntity.IssuingCode));
            var docResult = manageSQLConnection.GetDataSetValues("GetApprovalStatusForDocument", sqlParametersDocApproval);
            if (manageReport.CheckDocApproval(docResult))
            {
                if (documentStockIssuesEntity.Type == 2)
                {
                    ManageDocumentIssues documentIssues = new ManageDocumentIssues();
                    documentIssues.GenerateIssues(documentStockIssuesEntity);
                    if (documentStockIssuesEntity.Loadingslip == "N" || string.IsNullOrEmpty(documentStockIssuesEntity.Loadingslip))
                    {
                        List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                        sqlParameters.Add(new KeyValuePair<string, string>("@SINo", documentStockIssuesEntity.SINo));
                        manageSQLConnection.UpdateValues("UpdateStockIssuesLoadingslip", sqlParameters);
                    }
                    return new Tuple<bool, string, string>(true, "Print Generated Successfully", documentStockIssuesEntity.SINo);
                }
                else
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentStockIssuesEntity.IssuingCode));
                    var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
                    if (manageReport.CheckDataAvailable(result))
                    {
                        if (documentStockIssuesEntity.DocType == 2) //(documentStockIssuesEntity.SINo.Trim() != "0" && documentStockIssuesEntity.SINo.Trim() != "-")
                        {
                            List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                            sqlParameters1.Add(new KeyValuePair<string, string>("@Type", "2"));
                            sqlParameters1.Add(new KeyValuePair<string, string>("@DocNumber", documentStockIssuesEntity.SINo.Trim()));
                            var result1 = manageSQLConnection.GetDataSetValues("CheckDocumentEdit", sqlParameters1);
                            if (!manageReport.CheckDataAvailable(result1))
                            {
                                return new Tuple<bool, string, string>(false, GlobalVariable.DocumentEditPermission, "");
                            }
                            // CheckDocumentEdit
                        }
                        else
                        {
                            // check document available
                            List<KeyValuePair<string, string>> sqlParameterscheckdate = new List<KeyValuePair<string, string>>();
                            sqlParameterscheckdate.Add(new KeyValuePair<string, string>("@SINo", documentStockIssuesEntity.SINo));
                            var result1 = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsBySINo", sqlParameterscheckdate);
                            if (manageReport.CheckDataAvailable(result1))
                            {
                                return new Tuple<bool, string, string>(false, "This document number " + documentStockIssuesEntity.SINo + " is already exists, Please refresh the page.", "");
                            }
                        }
                        StockIssueMemo stockIssueMemo = new StockIssueMemo();
                        return stockIssueMemo.InsertStockIssueData(documentStockIssuesEntity);

                    }
                    else
                    {
                        return new Tuple<bool, string, string>(false, "Permission not Granted", "");
                    }
                }
            }
            else
            {
                return new Tuple<bool, string, string>(false, "Please Approve CB, Receipt, Issues and Truck memo for yesterday documents. If you wants to do without approval Please get permission from HO (MD).", "");
            }
        }

        [HttpGet("{id}")]
        public string Get(string value, int Type, string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SIDate", value));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsByDate", sqlParameters);
            }
            else if (Type == 2)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SINo", value));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsBySINo", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", value));
                ds = manageSQLConnection.GetDataSetValues("GetGatePass", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds);
        }

        [HttpPut("{id}")]
        public bool Put(PrintEntity entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@SINo", entity.DOCNumber));
            return manageSQLConnection.UpdateValues("UpdateStockIssuesLoadingslip", sqlParameters);
        }

    }
}