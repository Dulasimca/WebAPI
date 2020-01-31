using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIssuesAbstractPrintController : ControllerBase
    {
        [HttpPost("id")]
        public bool GeneratePrint(List<string> DocumentId)
        {
            string documentid = string.Empty;
            foreach (string item in DocumentId)
            {
                documentid = documentid + "~" + item;
            }
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@DocumenteId", documentid));
            ds = manageSQLConnection.GetDataSetValues("GetStockIssueDetailsByDate", sqlParameters);

            return false;

        }
    }
}