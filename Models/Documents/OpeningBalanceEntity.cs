using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.Documents
{
    public class OpeningBalanceEntity
    {
    //    public string Rowid { get; set; }
        public string GodownCode { get; set; }
        public string ObDate { get; set; }
        public string RegionCode { get; set; }
        public string CommodityCode { get; set; }
        public string BookBalanceBags { get; set; }
        public string BookBalanceWeight { get; set; }
        public string PhysicalBalanceBags { get; set; }
        public string PhysicalBalanceWeight { get; set; }
        public string CumulitiveShortage { get; set; }
    }

    public class StackOpeningEntity
    {
      public string  GodownCode { get; set; }
      public string  CommodityCode { get; set; }
       public string  StackNo { get; set; }
        public string CurrYear { get; set; }
       public string Bags { get; set; }
       public string Weights { get; set; }
       public string ObStackDate { get; set; }
       public string ExportFlag { get; set; }
       public string RegionCode { get; set; }
       public string Flag1 { get; set; }
       public string Flag2 { get; set; }
      // public DateTime clstackdate { get; set; }
    }
}
