namespace TNCSCAPI
{
    public class GlobalVariable
    {
        //Testing connection
        public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id = sqladmin; password =sql@svc&ac!72;";
        public const string ReportPath = "C://LocalRepository//TNCSCUI//dist//GoodsStock//assets//";

        //Live connection
        //public const string ConnectionString = "data source=localhost;initial catalog=TNCSCSCM;user id = sqladmin; password =sql@svc&ac!34;";
        //public const string ReportPath = "C://Repos//LiveCode//TNCSCWebSite//assets//";

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
        public const string QuantityAccountIssues = "ISQAC";
        public const string QuantityAccountReceipt = "REQAC";

        public const string QAIssuesForAllScheme = "ISQASCHEME";
        public const string QAIssuesForAllSchemeCRS = "ISQACRS";
        public const string QAIssuesForAllSchemeSociety = "ISQASOCIETY";
        public const string QAReceiptForAllScheme = "REQASCHEME";
        public const string QATruckMemoForAllScheme = "TMQASCHEME";

        public const string HullingDetailsReportFileName = "";
        public const string SchemeReceiptReportFileName = "";
        public const string CommodityReceiptReportFileName = "COMREC";
        public const string CommodityIssueMemoReportFileName = "COMIM";

        public const string SavedMessage = "Saved Successfully! : ";
        public const string ErrorMessage = "Please Contact Administrator.";
    }
}
