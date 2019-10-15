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
        public Tuple<bool, string> Post(DocCorrectionEntity docCorrectionEntity = null)
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
                var result = manageSQLConnection.InsertData("InsertDocumentCorrection", sqlParameters);
                if (result)
                {
                    return new Tuple<bool, string>(true, "Request sent!");
                }
                else
                {
                    return new Tuple<bool, string>(false, "Please contact Administrator");
                }
            }
            else
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Id", docCorrectionEntity.Id.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@ApproverRoleID", docCorrectionEntity.ApproverRoleID.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@ApproverReason", docCorrectionEntity.ApproverReason.ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@ApprovalStatus", docCorrectionEntity.ApprovalStatus.ToString()));
                var result = manageSQLConnection.UpdateValues("UpdateDocumentCorrection", sqlParameters);
                if (result)
                {
                    return new Tuple<bool, string>(true, docCorrectionEntity.ApprovalStatus == 1 ? "Approved!" : "Rejected!");
                }
                else
                {
                    return new Tuple<bool, string>(false, "Please contact Administrator");
                }
            }
            // return new Tuple<bool, string>(false, "Please contact Administrator");
        }

        [HttpGet("{id}")]
        public string Get(string Value, string Code, int Type,string ToDate)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@Value", Value));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", Code));
            sqlParameters.Add(new KeyValuePair<string, string>("@Type", Type.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            ds = manageSQLConnection.GetDataSetValues("GetDocumentCorrection", sqlParameters);
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }

            return JsonConvert.SerializeObject("");
        }
    }
    public class DocCorrectionEntity
    {
        public int Id { get; set; }
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

