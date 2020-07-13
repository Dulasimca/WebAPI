using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateDocumentNoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DocumentNoEntity documentNoEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (documentNoEntity.DocType == 1)// issues
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@SIDate", documentNoEntity.SIDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@IssuingCode", documentNoEntity.IssuingCode));
                ds = manageSQLConnection.GetDataSetValues("GenerateSINO", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class DocumentNoEntity
    {
        public string SIDate { get; set; }
        public string IssuingCode { get; set; }
        public int DocType { get; set; }
    }
}