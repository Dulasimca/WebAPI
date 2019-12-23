using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.ManageSQL;

namespace TNCSCAPI.Controllers.Allotment
{
    [Route("api/[controller]")]
    [ApiController]
    public class FPSAllotmentController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post([FromBody]List<FPSAllotmentQuantityEntity> allotmentEntity)
        {
            ManageSQLForAllotmentQty manageSQL = new ManageSQLForAllotmentQty();
            return manageSQL.InsertFPSAllotmentQty(allotmentEntity);
        }
    }

    public class FPSAllotmentQuantityEntity
    {
        public string FPSCode { get; set; }
        public string GCode { get; set; }
        public string Taluk { get; set; }
        public string AllotmentMonth { get; set; }
        public string AllotmentYear { get; set; }
        public List<FPSAllotmentQtyItemList> ItemList { get; set; }
    }
    public class FPSAllotmentQtyItemList
    {
        public string Commodity { get; set; }
        public string Scheme { get; set; }
        public string Quantity { get; set; }
    }
}