using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.Purchase
{
    public class TenderDetailsEntity
    {
            public int Type { get; set; }
            public string TenderDetId { get; set; }
            public string TenderId { get; set; }
            public string TenderDate { get; set; }
            public string CompletedDate { get; set; }
            public string OrderNumber { get; set; }
            public string OrderDate { get; set; }
            public string ITCode { get; set; }
            public string Quantity { get; set; }
            public string AdditionalQty { get; set; }
            public string Remarks { get; set; }

    }

    public class TenderAllotmentDetailsEntity
    {
        public string TenderDetId { get; set; }
        public string AllotmentID { get; set; }
        public string PartyCode { get; set; }
        public string TotalDays { get; set; }
        public string TargetDate { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }
    }

    public class TenderAllotmentToRegionEntity
    {
        public string RegAllotmentID { get; set; }
        public string TenderAllotmentID { get; set; }
        public string RCode { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }

    }
}
