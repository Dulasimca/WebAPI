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
    public class AllotmentQuantityController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post([FromBody]List<AllotmentQuantityEntity> allotmentEntity)
        {

            ManageSQLForAllotmentQty manageSQL = new ManageSQLForAllotmentQty();
            return manageSQL.InsertAllotmentQtyEntry(allotmentEntity);
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