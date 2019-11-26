using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.Allotment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllotmentQuantityController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post([FromBody]List<AllotmentQuantityEntity> allotmentEntity)
        {

            ManageSQLForAllotmentQty manageSQL = new ManageSQLForAllotmentQty();
            return manageSQL.InsertAllotmentQtyEntry(allotmentEntity);
        }
        [HttpGet("{id}")]
        public string Get(string RCode, string GCode, string Month, string Year, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            // sqlParameters.Add(new KeyValuePair<string, string>("@RoleId", RoleId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Gcode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Month", Month));
            sqlParameters.Add(new KeyValuePair<string, string>("@Year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", ToDate));
            ds = manageSQLConnection.GetDataSetValues("GetAllotmentIssueQuantityDetails", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
   

    public class AllotmentQuantityEntity
    {
        public string FPSName { get; set; }
        public string FPSCode { get; set; }
        public string SocietyCode { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string Taluk { get; set; }
        public string AllotmentMonth { get; set; }
        public string AllotmentYear { get; set; }
        public List<AllotmentQtyItemList> ItemList { get; set; }
    }
    public class AllotmentQtyItemList
    {
        public string ITCode { get; set; }
        public string ITName { get; set; }
        public string Quantity { get; set; }
    }
}