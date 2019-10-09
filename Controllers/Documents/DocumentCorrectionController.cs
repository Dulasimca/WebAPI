using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentCorrectionController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(DocCorrectionEntity docCorrectionEntity = null)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (docCorrectionEntity.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", docCorrectionEntity.DocNo));
                sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", docCorrectionEntity.RoleId.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@DocType", docCorrectionEntity.DocType.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@RegionCode", docCorrectionEntity.RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", docCorrectionEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Reason", docCorrectionEntity.Reason));
                return manageSQLConnection.InsertData("InsertDocumentCorrection", sqlParameters);
            }
            return false;
        }

        [HttpGet("{id}")]
        public string Get(string DocNo, int Type)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", DocNo));
                ds = manageSQLConnection.GetDataSetValues("GetDocumentCorrection", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(ds))
                {
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
            return  JsonConvert.SerializeObject(""); 
        }
    }
    public class DocCorrectionEntity
    {
        public int Type { get; set; }
        public string DocNo { get; set; }
        public int RoleId { get; set; }
        public int DocType { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApproverRoleID { get; set; }
        public string ApproverReason { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int UpdateStatus { get; set; }
    }
}

 