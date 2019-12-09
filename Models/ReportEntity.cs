using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models
{
    public class PurchaseParameter
    {
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OrderNo { get; set; }
        public string UserName { get; set; }
    }

    public class GUGRParameter
    {
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
    }
}
