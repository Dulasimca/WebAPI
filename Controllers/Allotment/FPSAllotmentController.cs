using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.ManageSQL;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Allotment
{
    [Route("api/[controller]")]
    [ApiController]
    public class FPSAllotmentController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] FPSAllotmentQuantityEntity  allotmentEntity)
        {
            ManageSQLForAllotmentQty manageSQL = new ManageSQLForAllotmentQty();
           var result= manageSQL.InsertFPSAllotmentQty(allotmentEntity);
            return JsonConvert.SerializeObject(result);
        }
    }
    public class StatusMessages
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class FPSAllotmentQuantityEntity
    {
        public string AllotmentMonth { get; set; }
        public string AllotmentYear { get; set; }
        public List<ItemDetails> ItemDetail { get; set; }
    }
    public class ItemDetails
    {
        public string FPSCode { get; set; }
        public string GodownCode { get; set; }
        public string TalukName { get; set; }
        public List<FPSAllotmentQtyItemList> CommodityDetaills { get; set; }
    }
    public class FPSAllotmentQtyItemList
    {
        public string CommodityName { get; set; }
        public string SchemeName { get; set; }
        public string Quantity { get; set; }
    }
}