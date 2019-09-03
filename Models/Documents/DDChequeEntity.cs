using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.Documents
{
    public class DDChequeEntity
    {
        public string GCode { get; set; } //godcode
        public string ReceiptNo { get; set; } //vrno
        public string Details { get; set; } //detail
        public string Flag { get; set; } //eflag
        public string UserID { get; set; }
        public string GodownName { get; set; }
        public string RegionName { get; set; }
        public string Total { get; set; }
        public List<ReceiptChequedetailEntity> DDChequeItems { get; set; }

    }
    public class ReceiptChequedetailEntity
    {
        public string ChequeNo { get; set; } //cdno
        public string ChequeDate { get; set; } //cddate
        public string ReceiptDate { get; set; } //recdate
        public string PaymentType { get; set; } //cdopt
        public string ReceivedFrom { get; set; } //cdwhom
        public string Amount { get; set; } //cdamount
        public string ReceivorCode { get; set; } //whomcode
        public string Bank { get; set; } //cdbank
        public string Flag { get; set; } //eflag
    }
}
