using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.DeliveryOrder;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandDraftController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] DemandDraftEntity demandDraft)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", demandDraft.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", demandDraft.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", demandDraft.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", demandDraft.RCode));
            ds = manageSQLConnection.GetDataSetValues("Get_demanddraftdetails", sqlParameters);
            ManageDemandDraft manageDemand = new ManageDemandDraft();
            ManageReport manageReport = new ManageReport();
            //filter condotions
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = demandDraft.GCode,
                    FromDate = demandDraft.FromDate,
                    Todate = demandDraft.ToDate,
                    UserName = demandDraft.UserName,
                    GName = demandDraft.GName,
                    RName = demandDraft.RName
                };
                // commodityIssueMemo.GenerateCommodityIssueMemoReport(entity);
                Task.Run(() => manageDemand.GenerateDemandDraftReport(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class DemandDraftEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string UserName { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
    }
}