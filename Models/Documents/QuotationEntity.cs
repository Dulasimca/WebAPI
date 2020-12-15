using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.Documents
{
    public class QuotationEntity
    {
            public List<int> ProductID { get; set; }
            public string GCode { get; set; }
            public string RCode { get; set; }
            public string Remarks { get; set; }
            public string EmailID { get; set; }
            public string PhoneNo { get; set; }
            public string Products { get; set; }
    }
}
