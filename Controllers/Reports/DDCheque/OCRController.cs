using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.DDCheque;

namespace TNCSCAPI.Controllers.Reports.DDCheque
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post([FromBody] OCREntity oCREntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", oCREntity.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", oCREntity.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", oCREntity.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetOCRReport", sqlParameters);
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                ManageOCRReport manageOCR = new ManageOCRReport();
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = oCREntity.GCode,
                    FromDate = oCREntity.FromDate,
                    Todate = oCREntity.ToDate,
                    UserName = oCREntity.UserID,
                    GName = oCREntity.GName,
                    RName = oCREntity.RName
                };
                Task.Run(() => manageOCR.GenerateOCRReport(entity)); //Generate the Report
            }

            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }

    public class OCREntity
    {
        public string GCode { get; set; }
        public string UserID { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}