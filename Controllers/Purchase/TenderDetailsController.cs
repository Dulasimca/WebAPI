using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(TenderDetailsEntity tenderDetailsEntity = null)
        {
            //if (tenderDetailsEntity.Type == 2)
            //{
            //    ManageSQLForTenderDetails manageSQLConnection = new ManageSQLForTenderDetails();
            //    return manageSQLConnection.InsertTenderDetails(tenderDetailsEntity);
            //}
            //else
            //{
                ManageSQLForTenderDetails manageSQLConnection = new ManageSQLForTenderDetails();
                return manageSQLConnection.InsertTenderDetails(tenderDetailsEntity);
            //}
        }
        [HttpGet("{id}")]
        public string Get(string value, int Type)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                if (Type == 2)
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@OrderNumber", value));
                    ds = manageSQLConnection.GetDataSetValues("GetTenderQuantityDetails", sqlParameters);
                }
                else
                {
                    ds = manageSQLConnection.GetDataSetValues("GetTenderDetails");
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}
