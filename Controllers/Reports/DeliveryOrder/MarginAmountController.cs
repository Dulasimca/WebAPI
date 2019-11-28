using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.DeliveryOrder;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarginAmountController
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSchemeWiseEntity marginAmount)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", marginAmount.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", marginAmount.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", marginAmount.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", marginAmount.RCode));
            // sqlParameters.Add(new KeyValuePair<string, string>("@SocCode", marginAmount.SCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrderMarginAmount", sqlParameters);
            ManageDOMargin manageDOMargin = new ManageDOMargin();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = marginAmount.GCode,
                    FromDate = marginAmount.FromDate,
                    Todate = marginAmount.ToDate,
                    UserName = marginAmount.UserName,
                    GName = marginAmount.GName,
                    RName = marginAmount.RName
                };
                // commodityIssueMemo.GenerateCommodityIssueMemoReport(entity);
                Task.Run(() => manageDOMargin.GenerateDOMarginReport(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    //public class MarginAmountEntity
    //{
    //    public string FromDate { get; set; }
    //    public string ToDate { get; set; }
    //    public string GCode { get; set; }
    //   // public string SCode { get; set; }
    //}
}