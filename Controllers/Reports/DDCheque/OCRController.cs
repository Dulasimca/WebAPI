using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                //CommonEntity entity = new CommonEntity
                //{
                //    dataSet = ds,
                //    GCode = reportParameter.GCode,
                //    FromDate = reportParameter.FromDate,
                //    Todate = reportParameter.ToDate,
                //    UserName = reportParameter.UserName
                //};
                //Task.Run(() => stockDeliveryOrder.GenerateDeliveryOrderForRegister(entity)); //Generate the Report
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