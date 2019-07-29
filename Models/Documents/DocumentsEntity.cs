using System;
using System.Collections.Generic;

namespace TNCSCAPI.Models.Documents
{
    public class DocumentStockReceiptList
    {
        public string SRNo { get; set; }
        public string RowId { get; set; }
        public DateTime SRDate { get; set; }
        public string PAllotment { get; set; }
        public string MTransport { get; set; }
        public string Trcode { get; set; }
        public string DepositorType { get; set; }
        public string DepositorCode { get; set; }
        public string TruckMemoNo { get; set; }
        public DateTime TruckMemoDate { get; set; }
        public string ManualDocNo { get; set; }
        public string LNo { get; set; }
        public string LFrom { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string ReceivingCode { get; set; }
        public string RCode { get; set; }
        public string Remarks { get; set; }

        public string GodownName { get; set; }
        public string RegionName { get; set; }
        public string TransactionType { get; set; }
        public string DepositorName { get; set; }
        public string UnLoadingSlip { get; set; }
        public string UserID { get; set; }

        public List<StockReceiptItemList> ItemList { get; set; }
    }

    public class StockReceiptItemList
    {
        public string TStockNo { get; set; }
        public string Scheme { get; set; }
        public string ICode { get; set; }
        public string IPCode { get; set; }
        public int NoPacking { get; set; }
        public double GKgs { get; set; }
        public double Nkgs { get; set; }
        public string WTCode { get; set; }
        public float Moisture { get; set; }

        public string CommodityName { get; set; }
        public string SchemeName { get; set; }
        public string PackingName { get;set; }
    }

    public class DocumentStockIssuesEntity
    {
        public string SINo { get; set; }
        public string RowId { get; set; }
        public DateTime SIDate { get; set; }
        public string Trcode { get; set; }
        public string DNo { get; set; }
        public string DDate { get; set; }
        public string WCCode { get; set; }
        public string IssuingCode { get; set; }
        public string RCode { get; set; }
        public string Receivorcode { get; set; }
        public string Issuetype { get; set; }
        public string IRelates { get; set; }
        public string Remarks { get; set; }
        public string TransporterName { get; set; }
        public decimal TransportingCharge { get; set; }
        public string LorryNo { get; set; }
        public double NewBale { get; set; }
        public double SoundServiceable { get; set; }
        public double ServiceablePatches { get; set; }
        public string issuetype1 { get; set; }
        public string ExportFlag { get; set; }
        public int GunnyUtilised { get; set; }
        public int GunnyReleased { get; set; }
        public string Loadingslip { get; set; }
        public string IssueMemo { get; set; }
        public string ManualDocNo { get; set; }
        public string IssueRegularAdvance { get; set; }
        public List<DocumentStockIssuesItemEntity> IssueItemList { get; set; }
        public List<DocumentStockIssueDetailsEntity> SIDetailsList { get; set; }
    }

    public class DocumentStockIssuesItemEntity
    {
        public string TStockNo { get; set; }
        public string ICode { get; set; }
        public string IPCode { get; set; }
        public int NoPacking { get; set; }
        public string WTCode { get; set; }
        public double GKgs { get; set; }
        public double Nkgs { get; set; }
        public double Moisture { get; set; }
        public string Scheme { get; set; }
    }

    public class DocumentStockIssueDetailsEntity
    {
        public DateTime DDate { get; set; }
        public string DNo { get; set; }
        public string SINo { get; set; }
        public DateTime SIDate { get; set; }
        public string GodownCode { get; set; }
        public string RCode { get; set; }
    }


    public class DocumentDeliveryOrderEntity
    {
        public string Dono { get; set; }
        public string RowId { get; set; }
        public DateTime DoDate { get; set; }
        public string TransactionCode { get; set; }
        public string IndentNo { get; set; }
        public DateTime PermitDate { get; set; }
        public string OrderPeriod { get; set; }
        public string ReceivorCode { get; set; }
        public string IssuerCode { get; set; }
        public string IssuerType { get; set; }
        public double GrandTotal { get; set; }
        public string Regioncode { get; set; }
        public string Remarks { get; set; }
        public string deliverytype { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
        public DateTime dotime { get; set; }

        public string GodownName { get; set; }
        public string TransactionName { get; set; }
        public string RegionName { get; set; }
        public string UnLoadingSlip { get; set; }
        public string UserID { get; set; }
        
        public List<DocumentDeliveryItemDetails> documentDeliveryItems { get; set; }
        public List<DocumentDeliveryAdjustmentDetails> deliveryAdjustmentDetails { get; set; }
        public List<DocumentDeliveryPaymentDetails> deliveryPaymentDetails { get; set; }
        public List<DocumentDeliveryMarginDetails> deliveryMarginDetails { get; set; }
    }
    public class DocumentDeliveryItemDetails
    {
        public string Itemcode { get; set; }
        public double NetWeight { get; set; }
        public string Wtype { get; set; }
        public string Scheme { get; set; }
        public double Rate { get; set; }
        public double Total { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }
    public class DocumentDeliveryAdjustmentDetails
    {
        public string AdjustedDoNo { get; set; }
        public double Amount { get; set; }
        public string AdjustmentType { get; set; }
        public DateTime AdjustDate { get; set; }
        public double AmountNowAdjusted { get; set; }
        public double Balance { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }
    public class DocumentDeliveryPaymentDetails
    {
        public string PaymentMode { get; set; }
        public double PaymentAmount { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChDate { get; set; }
        public string bank { get; set; }
        public string payableat { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }
    public class DocumentDeliveryMarginDetails
    {
        public string SchemeCode { get; set; }
        public string ItemCode { get; set; }
        public string MarginWtype { get; set; }
        public double MarginNkgs { get; set; }
        public double MarginRate { get; set; }
        public double MarginAmount { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }

    public class DocumentStockTransferDetails
    {
        public string STNo { get; set; }
        public string RowId { get; set; }
        public DateTime STDate { get; set; }
        public string TrCode { get; set; }
        public string MNo { get; set; }
        public DateTime MDate { get; set; }
        public string RNo { get; set; }
        public DateTime RDate { get; set; }
        public string LorryNo { get; set; }
        public string ReceivingCode { get; set; }
        public string IssuingCode { get; set; }
        public string RCode { get; set; }
        public int GunnyUtilised { get; set; }
        public int GunnyReleased { get; set; }
        public string IssueSlip { get; set; }
        public string TruckMemo { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }

        public string GodownName { get; set; }
        public string TransactionName { get; set; }
        public string ReceivingName { get; set; }
        public string RegionName { get; set; }
       // public string UnLoadingSlip { get; set; }
        public string UserID { get; set; }

        public List<DocumentSTItemDetails> documentSTItemDetails { get; set; }
        public List<DocumentSTTDetails> documentSTTDetails { get; set; }
    }

    public class DocumentSTItemDetails
    {
        public string TStockNo { get; set; }
        public string ICode { get; set; }
        public string IPCode { get; set; }
        public int NoPacking { get; set; }
        public string WTCode { get; set; }
        public double GKgs { get; set; }
        public double Nkgs { get; set; }
        public double Moisture { get; set; }
        public string Scheme { get; set; }
        public string RCode { get; set; }
        public string ExportFlag { get; set; }
        public string flag1 { get; set; }
        public string Flag2 { get; set; }
    }

    public class DocumentSTTDetails
    {
        public string TransportMode { get; set; }
        public string TransporterName { get; set; }
        public string LWBillNo { get; set; }
        public DateTime LWBillDate { get; set; }
        public double FreightAmount { get; set; }
        public double Kilometers { get; set; }
        public string WHDNo { get; set; }
        public double WCharges { get; set; }
        public double HCharges { get; set; }
        public string FStation { get; set; }
        public string TStation { get; set; }
        public string Remarks { get; set; }
        public string FCode { get; set; }
        public string Vcode { get; set; }
        public DateTime LDate { get; set; }
        public string LNo { get; set; }
        public string Wno { get; set; }
        public string RRNo { get; set; }
        public string RailHead { get; set; }
        public double RFreightAmount { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }

}
