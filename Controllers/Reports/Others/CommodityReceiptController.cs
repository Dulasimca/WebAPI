using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports;

namespace TNCSCAPI.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityReceiptController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] CommodityParameter commodity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FDate", commodity.FDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", commodity.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", commodity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", commodity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@TRCODE", commodity.TRCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", commodity.ITCode));
            ds = manageSQLConnection.GetDataSetValues("GetCOMMODITYRECEIPT", sqlParameters);
            //
            CommodityReceipt commodityReceipt = new CommodityReceipt();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = commodity.GCode,
                    FromDate = commodity.FDate,
                    Todate = commodity.ToDate,
                    UserName = commodity.UserName,
                  //  SchemeName = commodity.SchemeName,
                };
                Task.Run(() => commodityReceipt.GenerateCommodityReceiptReport(entity)); //Generate the Report
            }

            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class CommodityParameter
    {
        public string FDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string TRCode { get; set; }
        public string ITCode { get; set; }
        public string UserName { get; set; }
        public int IssueToGodown { get; set; }
        public int IssueToDepositor { get; set; }
    }
}