using System.Collections.Generic;

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
        public string GName { get; set; }
        public string RNname { get; set; }
    }
}
