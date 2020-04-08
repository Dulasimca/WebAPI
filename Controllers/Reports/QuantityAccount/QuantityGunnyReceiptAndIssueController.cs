using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.QA;

namespace TNCSCAPI.Controllers.Reports.QuantityAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuantityGunnyReceiptAndIssueController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(QuantityAccountEntity quantityAccountEntity)
        {
            // DataSet ds = new DataSet();
            ManageQuantityGunnyReceiptandIssues manageQuantityAccount = new ManageQuantityGunnyReceiptandIssues();
            var result = manageQuantityAccount.ProcessQAStatement(quantityAccountEntity);
            return JsonConvert.SerializeObject(result);
        }
    }

}