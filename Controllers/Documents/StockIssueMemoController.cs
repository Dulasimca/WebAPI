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
            if (documentStockIssuesEntity.Type == 2)
            {
                ManageDocumentIssues documentIssues = new ManageDocumentIssues();
                documentIssues.GenerateIssues(documentStockIssuesEntity);
                if (documentStockIssuesEntity.Loadingslip == "N" || string.IsNullOrEmpty(documentStockIssuesEntity.Loadingslip))
                {
                    ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@SINo", documentStockIssuesEntity.SINo));
                    manageSQLConnection.UpdateValues("UpdateStockIssuesLoadingslip", sqlParameters);
                }
                return new Tuple<bool, string, string>(true, "Print Generated Successfully", documentStockIssuesEntity.SINo);
            }
            else
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentStockIssuesEntity.IssuingCode));
                var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(result))
                {
                    if (documentStockIssuesEntity.SINo.Trim() != "0" && documentStockIssuesEntity.SINo.Trim() != "-")
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
                    StockIssueMemo stockIssueMemo = new StockIssueMemo();
                    return stockIssueMemo.InsertStockIssueData(documentStockIssuesEntity);
                }
                else
                {
                    return new Tuple<bool, string, string>(false, "Permission not Granted", "");
                }
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
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SINo", value));
                ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsBySINo", sqlParameters);
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