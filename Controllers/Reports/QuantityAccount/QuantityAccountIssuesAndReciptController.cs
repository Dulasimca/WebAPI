using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TNCSCAPI.ManageAllReports.QA;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Reports.QuantityAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuantityAccountIssuesAndReciptController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(QuantityAccountEntity quantityAccountEntity)
        {
            // DataSet ds = new DataSet();
            ManageQuantityAccountReceiptandIssues manageQuantityAccount = new ManageQuantityAccountReceiptandIssues();
            var result=manageQuantityAccount.ProcessQAStatement(quantityAccountEntity);
            return JsonConvert.SerializeObject(result);
        }
    }

}