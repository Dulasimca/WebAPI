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
        public float BookBalanceBags { get; set; }
        public float BookBalanceWeight { get; set; }
        public float PhysicalBalanceBags { get; set; }
        public float PhysicalBalanceWeight { get; set; }
        public float CumulitiveShortage { get; set; }
    }

    public class StackOpeningEntity
    {
      public string  GodownCode { get; set; }
      public string  CommodityCode { get; set; }
       public string  StackNo { get; set; }
       public double StackBalanceBags { get; set; }
       public double StackBalanceWeight { get; set; }
       public DateTime ObStackDate { get; set; }
       public string ExportFlag { get; set; }
       public string RegionCode { get; set; }
      public string Flag1 { get; set; }
       public string Flag2 { get; set; }
       public DateTime clstackdate { get; set; }
    }
}
