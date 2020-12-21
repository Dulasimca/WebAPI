using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.DoToSales
{
    public class DOSalesTaxEntity
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string Itemcode { get; set; }
        public string GSTNumber { get; set; }
        public string PartyId { get; set; }
        public string Dono { get; set; }
        public string DoDate { get; set; }
        public string Wtype { get; set; }
        public string TaxPercentage { get; set; }
        public float NetWeight { get; set; }
        public float SalesRate { get; set; }
        public float SalesTOTAL { get; set; }
        public float SGST { get; set; }
        public float CGST { get; set; }
        public string Hsncode { get; set; }
        public float GSTTOTAL { get; set; }
        public float TotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public string Scheme { get; set; }
        public string dotime { get; set; }
        public string Regioncode { get; set; }
        public string CurrentDate { get; set; }
        public string TransactionCode { get; set; }
        public string IssuerCode { get; set; }
        public float Rate { get; set; }
        public float Amount { get; set; }
        public string AccYear { get; set; }
        public float DITotal { get; set; }
    }
}
