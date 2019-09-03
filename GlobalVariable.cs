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

        public const string SavedMessage = "Saved Successfully! : ";
        public const string ErrorMessage = "Please Contact Administrator.";
    }
}
