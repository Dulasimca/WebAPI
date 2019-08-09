using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports.Stack;

namespace TNCSCAPI.Controllers.Documents.TransactionStatus
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionStatusDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(TransactionEntity entity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> listParameters = new List<KeyValuePair<string, string>>();
            if (entity.Type == 1)
            {
                listParameters.Add(new KeyValuePair<string, string>("@Docdate", entity.Docdate));
                listParameters.Add(new KeyValuePair<string, string>("@Gcode", entity.Gcode));
                listParameters.Add(new KeyValuePair<string, string>("@RoleId", entity.RoleId));
                ds = manageSQLConnection.GetDataSetValues("GetTransactionstatus", listParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                ManageStackCard manageStack = new ManageStackCard();
                listParameters.Add(new KeyValuePair<string, string>("@Docdate", entity.Docdate));
                listParameters.Add(new KeyValuePair<string, string>("@RCode", entity.RCode));
                listParameters.Add(new KeyValuePair<string, string>("@RoleId", entity.RoleId));
                ds = manageSQLConnection.GetDataSetValues("GetTransactionStatusByDate", listParameters);
                //Manage Transactionstatus
                var result = manageStack.ManageApprovalStatus(ds, entity.Docdate);
                return JsonConvert.SerializeObject(result);
            }
        }
    }
}