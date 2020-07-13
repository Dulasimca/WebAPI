using System;
using System.Collections.Generic;

namespace TNCSCAPI.Models.Documents
{
    public class DocumentStockReceiptList
    {
        public string SRNo { get; set; }
        public string RowId { get; set; }
        public string SRDate { get; set; }
        public string PAllotment { get; set; }
        public string MTransport { get; set; }
        public string Trcode { get; set; }
        public string DepositorType { get; set; }
        public string DepositorCode { get; set; }
        public string TruckMemoNo { get; set; }
        public string TruckMemoDate { get; set; }
        public string ManualDocNo { get; set; }
        public string LNo { get; set; }
        public string LFrom { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string ReceivingCode { get; set; }
        public string RCode { get; set; }
        public string Remarks { get; set; }

        public string GodownName { get; set; }
        public string RegionName { get; set; }
        public string TransactionName { get; set; }
        public string TransporterName { get; set; }
        public string LWBNo { get; set; }
        public string LWBDate { get; set; }
        public string LDate { get; set; }
        //public string DepositorType { get; set; }
        public string DepositorName { get; set; }
        public string UnLoadingSlip { get; set; }
        public string UserID { get; set; }

        public int Type { get; set; }

        public List<StockReceiptItemList> ItemList { get; set; }
    }

    public class StockReceiptItemList
    {
        public string TStockNo { get; set; }
        public string Scheme { get; set; }
        public string ICode { get; set; }
        public string IPCode { get; set; }
        public int NoPacking { get; set; }
        public string GKgs { get; set; }
        public string Nkgs { get; set; }
        public string WTCode { get; set; }
        public string Moisture { get; set; }

        public string CommodityName { get; set; }
        public string SchemeName { get; set; }
        public string PackingName { get;set; }
        public string StackYear { get; set; }
    }

    public class DocumentStockIssuesEntity
    {
        public string SINo { get; set; }
        public string RowId { get; set; }
        public string SIDate { get; set; }
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
        public string TransportingCharge { get; set; }
        public string LorryNo { get; set; }
        public string NewBale { get; set; }
        public string SoundServiceable { get; set; }
        public string ServiceablePatches { get; set; }
        public string issuetype1 { get; set; }
        public string ExportFlag { get; set; }
        public int GunnyUtilised { get; set; }
        public int GunnyReleased { get; set; }
        public string Loadingslip { get; set; }
        public string IssueMemo { get; set; }
        public string ManualDocNo { get; set; }
        public string IssueRegularAdvance { get; set; }

        public string GodownName { get; set; }
        public string RegionName { get; set; }
        public string TransactionType { get; set; }
        public string ReceiverName { get; set; }
        public string IssuerCode { get; set; }
        public string UserID { get; set; }

        public int Type { get; set; }
        public int DocType { get; set; }
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
        public string GKgs { get; set; }
        public string Nkgs { get; set; }
        public string Moisture { get; set; }
        public string Scheme { get; set; }

        public string CommodityName { get; set; }
        public string SchemeName { get; set; }
        public string PackingName { get; set; }
        public string StackYear { get; set; }
    }
    

    public class DocumentStockIssueDetailsEntity
    {
        public string DDate { get; set; }
        public string DNo { get; set; }
        public string SINo { get; set; }
        public string SIDate { get; set; }
        public string GodownCode { get; set; }
        public string RCode { get; set; }
    }


    public class DocumentDeliveryOrderEntity
    {
        public string Dono { get; set; }
        public string RowId { get; set; }
        public string DoDate { get; set; }
        public string TransactionCode { get; set; }
        public string IndentNo { get; set; }
        public string PermitDate { get; set; }
        public string OrderPeriod { get; set; }
        public string ReceivorCode { get; set; }
        public string IssuerCode { get; set; }
        public string IssuerType { get; set; }
        public string GrandTotal { get; set; }
        public string Regioncode { get; set; }
        public string Remarks { get; set; }
        public string deliverytype { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
        public string dotime { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string PartyID { get; set; }

        public string GodownName { get; set; }
        public string TransactionName { get; set; }
        public string RegionName { get; set; }
        public string UserID { get; set; }
        public string ReceivorName { get; set; }
        public int Type { get; set; }
        public string DOTaxStatus { get; set; }

        public List<DocumentDeliveryItemDetails> documentDeliveryItems { get; set; }
        public List<DocumentDeliveryAdjustmentDetails> deliveryAdjustmentDetails { get; set; }
        public List<DocumentDeliveryPaymentDetails> deliveryPaymentDetails { get; set; }
        public List<DocumentDeliveryMarginDetails> deliveryMarginDetails { get; set; }
    }
    public class DocumentDeliveryItemDetails
    {
        public string Itemcode { get; set; }
        public string NetWeight { get; set; }
        public string Wtype { get; set; }
        public string Scheme { get; set; }
        public string Rate { get; set; }
        public string Total { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
        public string ITDescription { get; set; }
        public string SchemeName { get; set; }
        public string TaxPercent { get; set; }
        public string HsnCode { get; set; }
    }
    public class DocumentDeliveryAdjustmentDetails
    {
        public string AdjustedDoNo { get; set; }
        public string Amount { get; set; }
        public string AdjustmentType { get; set; }
        public string AdjustDate { get; set; }
        public string AmountNowAdjusted { get; set; }
        public string Balance { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }
    public class DocumentDeliveryPaymentDetails
    {
        public string PaymentMode { get; set; }
        public string PaymentAmount { get; set; }
        public string ChequeNo { get; set; }
        public string ChDate { get; set; }
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
        public string MarginNkgs { get; set; }
        public string MarginRate { get; set; }
        public string MarginAmount { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
        public string ITDescription { get; set; }
        public string SchemeName { get; set; }
    }

    public class DocumentStockTransferDetails
    {
        public string STNo { get; set; }
        public string RowId { get; set; }
        public string STDate { get; set; }
        public string TrCode { get; set; }
        public string MNo { get; set; }
        public string MDate { get; set; }
        public string RNo { get; set; }
        public string RDate { get; set; }
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
        public string ManualDocNo { get; set; }
        public string RailHeadName { get; set; }

        public int Type { get; set; }

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
        public string GKgs { get; set; }
        public string Nkgs { get; set; }
        public string Moisture { get; set; }
        public string Scheme { get; set; }
        public string RCode { get; set; }
        public string ExportFlag { get; set; }
        public string flag1 { get; set; }
        public string Flag2 { get; set; }
        public string ITDescription { get; set; }
        public string PackingType { get; set; }
        public string SchemeName { get; set; }
        public string StackYear { get; set; }
    }

    public class DocumentSTTDetails
    {
        public string TransportMode { get; set; }
        public string TransporterName { get; set; }
        public string LWBillNo { get; set; }
        public string LWBillDate { get; set; }
        public string FreightAmount { get; set; }
        public string Kilometers { get; set; }
        public string WHDNo { get; set; }
        public string WCharges { get; set; }
        public string HCharges { get; set; }
        public string FStation { get; set; }
        public string TStation { get; set; }
        public string Remarks { get; set; }
        public string FCode { get; set; }
        public string Vcode { get; set; }
        public string LDate { get; set; }
        public string LNo { get; set; }
        public string Wno { get; set; }
        public string RRNo { get; set; }
        public string RailHead { get; set; }
        public string RFreightAmount { get; set; }
        public string Rcode { get; set; }
        public string ExportFlag { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
    }

}
