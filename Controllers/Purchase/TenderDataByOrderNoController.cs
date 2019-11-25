using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderDataByOrderNoController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(OrderNoList list)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@OrderNumber", list.OrderNo));
                   // sqlParameters.Add(new KeyValuePair<string, string>("@RCode", list.RCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Type", list.Type));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Spell", list.Spell.ToString()));
                    sqlParameters.Add(new KeyValuePair<string, string>("@PartyCode", list.PartyCode.ToString()));
                    ds = manageSQLConnection.GetDataSetValues("GetTenderDataByOrderNumber", sqlParameters);
              
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }

    public class OrderNoList
    {
        public string OrderNo { get; set; }
        public string RCode { get; set; }
        public string Type { get; set; }
        public int Spell { get; set; }
        public int PartyCode { get; set; }
    }
}