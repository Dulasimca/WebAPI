namespace TNCSCAPI
{
    public class GlobalVariable
    {
        //Testing connection
        public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id = sqladmin; password =sql@svc&ac!72;";
        public const string ReportPath = "C://LocalRepository//TestingSite//TNCSCUI-Website//assets//";
        public const string ImagePath = "C://LocalRepository//TNCSCUI//dist//GoodsStock//assets//NotificationPopup//";
        public const string SignaturePath = "C://LocalRepository//TNCSCUI//dist//GoodsStock//assets//InchargeSignature//";
        public const string ConnectionStringTnDaily = "data source=localhost;initial catalog=tndaily;user id = sqladmin; password =sql@svc&ac!72;";

        //Live connection
        //public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id =sqladmin; password =sql@svc&ac!34;";
        //public const string ReportPath = "C://Repos//LiveCode//TNCSCWebSite//assets//";
        //public const string ImagePath = "C://Repos//LiveCode//TNCSCWebSite//assets//NotificationPopup//";
        //public const string SignaturePath = "C://Repos//LiveCode//TNCSCWebSite//assets//InchargeSignature//";
        //public const string ConnectionStringTnDaily = "data source=localhost;initial catalog=tndaily;user id = sqladmin; password =sql@svc&ac!34;";

        public const string StockDORegisterFileName = "DOREG";
        public const string StockTruckMemoRegisterFileName = "TMREG";
        public const string StockReceiptRegisterFileName = "REREG";
        public const string StockIssueRegisterFileName = "ISREG";
        public const string DocumentReceiptFileName = "REDOC";
        public const string DocumentIssueFileName = "ISDOC";
        public const string DocumentDOFileName = "DODOC";
        public const string DocumentTruckMemoFileName = "TMDOC";
        public const string DDChequeFileName = "DDCHE";

        public const string StackCardFileName = "SCARD";
        public const string StackCardRegister = "SCARDREG";
        public const string QuantityAccountIssues = "ISQAC";
        public const string QuantityAccountReceipt = "REQAC";

        public const string QAIssuesForAllScheme = "ISQASCHEME";
        public const string QAIssuesForAllSchemeCRS = "ISQACRS";
        public const string QAIssuesForAllSchemeSociety = "ISQASOCIETY";
        public const string QAReceiptForAllScheme = "REQASCHEME";
        public const string QATruckMemoForAllScheme = "TMQASCHEME";

        public const string HullingDetailsReportFileName = "HULREG";
        public const string SchemeReceiptReportFileName = "SCREC";
        public const string CommodityReceiptReportFileName = "COMREC";
        public const string WriteOFFReportFileName = "WOFF";
        public const string CommodityIssueMemoReportFileName = "COMISS";
        public const string SchemeIssueMemoReportFileName = "SCHEMEISS";
        public const string TransactionReceiptFileName = "TRREC";

        public const string DOAllSchemeReportFileName = "DOALLSCH";
        public const string DODemandDraftDateFileName = "DODDDATE";
        public const string DODemandDraftBankFileName = "DODDBANK";

        public const string DOMarginReportFileName = "DOMARGIN";
        public const string DOOAPReportFileName = "DOOAP";
        public const string DOAnnapornaReportFileName = "DOANNA";
        public const string DOOthersReportFileName = "DOOTHERS";
        public const string DOSPLPDSReportFileName = "DOSPLPDS";
        public const string DOSocietyReportFileName = "DOSOCIETY";

        public const string SalesIssueMemoFileName = "SIMCUSD";
        public const string SalesIssueMemoAbstractFileName = "SIMABSD";

        public const string OCRReportFileName = "OCRREG";
        public const string GSTFileName = "GST";

        public const string GUReportFileName = "GU";
        public const string GRReportFileName = "GR";

        public const string TruckToRegionFileName = "TRUCKTOREGION";
        public const string TruckFromRegionFileName = "TRUCKFROMREGION";
        public const string TruckTransitFileName = "TRUCKTRANSIT";

        public const string HoPurchaseFileName = "HOPURCHASE";
        public const string RoPurchaseFileName = "ROPURCHASE";
        public const string RoNoPurchaseFileName = "RONOPURCHASE";

        public const string IssueMemoSocietyAbstractFileName = "ISSUESOCIETYABSTRACT";
        public const string IssueMemoSocietyDateWiseFileName = "ISSUESOCIETYDATWISE";
        public const string IssueMemoSocietyDateAndSchemeFileName = "ISSUESOCIETYDATANDSCHEME";
        public const string IssueMemoSocietySchemeWiseFileName = "ISSUESOCIETYSCHEMEWISE";

        public const string StockStatementFileName = "STOCKSTATEMENT";
        public const string IssueGatePassFileName = "GATEPASS";
        public const string ReceiptAckFileName = "ACK";

        public const string SavedMessage = "Saved Successfully! : ";
        public const string ErrorMessage = "Please Contact Administrator.";
        public const string DocumentEditPermission = " Document Edit Permission is not Granted. Please Contact Administrator! ";
        public const string DocumentEditByHO = " This Document is Locked by HO. Please get approval from HO to update this document ";
        public const string DocumentEditPermissionForDO = " Document Edit Permission is not Granted. Please Contact Account Section! ";

        //FSSAI description
        public const string FSSAI1 = "  1. Received the Stock within FSSAI Limits / Uniform Specifications";
        public const string FSSAI2 = "  2. Received the Stock in Pest Free Conditions";
        public const string FSSAI3 = "  3. Received Jointly Signed Sealed Samples Satisfied with Good Quality and Quantity";
        public const string FSSAI4 = "  4. The stock was standardised, weighed and quality checked fully in the presence of ";
        public const string FSSAI5 = "     Mr____________________________ and issued for movement.";

        public const string FSSAISign = "                   Sign. of the Authorised Person. ";

    }
}
