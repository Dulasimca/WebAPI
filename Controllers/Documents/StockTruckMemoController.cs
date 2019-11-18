using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTruckMemoController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool,string,string> Post(DocumentStockTransferDetails documentStockTransfer = null)
        {
            if (documentStockTransfer.Type == 2)
            {
                ManageDocumentTruckMemo documentTruckMemo = new ManageDocumentTruckMemo();
                documentTruckMemo.GenerateTruckMemo(documentStockTransfer);
                if(documentStockTransfer.IssueSlip=="N" || string.IsNullOrEmpty(documentStockTransfer.IssueSlip))
                {
                    ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@STNo", documentStockTransfer.STNo));
                    manageSQLConnection.UpdateValues("UpdateStockTransferIssueslip", sqlParameters);
                }
                return new Tuple<bool, string,string>(true, "Print Generated Successfully", documentStockTransfer.STNo);
            }
            else
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", documentStockTransfer.IssuingCode));
                var result = manageSQLConnection.GetDataSetValues("AllowDocumentEntry", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(result))
                {
                    if (documentStockTransfer.STNo.Trim() != "0" && documentStockTransfer.STNo.Trim() != "-")
                    {
                        List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                        sqlParameters1.Add(new KeyValuePair<string, string>("@Type", "3"));
                        sqlParameters1.Add(new KeyValuePair<string, string>("@DocNumber", documentStockTransfer.STNo.Trim()));
                        var result1 = manageSQLConnection.GetDataSetValues("CheckDocumentEdit", sqlParameters1);
                        if (!manageReport.CheckDataAvailable(result1))
                        {
                            return new Tuple<bool, string, string>(false, GlobalVariable.DocumentEditPermission, "");
                        }
                        // CheckDocumentEdit
                    }
                    ManageTruckMemo manageTruck = new ManageTruckMemo();
                    return manageTruck.InsertTruckMemoEntry(documentStockTransfer);
                }
                else
                {
                    return new Tuple<bool, string,string>(false, "Permission not Granted","");
                }
            }
        }

        [HttpGet("{id}")]
        public string Get(string sValue, int Type,string GCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@STDate", sValue));
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetStockTransferDetailsByDate", sqlParameters);
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@STNo", sValue));
                ds = manageSQLConnection.GetDataSetValues("GetStockTransferDetailsBySTNO", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPut("{id}")]
        public bool Put(PrintEntity entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@STNo", entity.DOCNumber));
            return manageSQLConnection.UpdateValues("UpdateStockTransferIssueslip", sqlParameters);
        }

    }
}