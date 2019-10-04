using System;
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
    public class SchemeWiseController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSchemeWiseEntity SchemeWise)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", SchemeWise.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", SchemeWise.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", SchemeWise.GCode));
         //   sqlParameters.Add(new KeyValuePair<string, string>("@SocCode", SchemeWise.SCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrdersAllScheme", sqlParameters);
            ManageDOAllScheme manageDOAllScheme = new ManageDOAllScheme();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = SchemeWise.GCode,
                    FromDate = SchemeWise.FromDate,
                    Todate = SchemeWise.ToDate,
                    UserName = SchemeWise.UserID,
                    GName = SchemeWise.GName,
                    RName = SchemeWise.RName
                };
                manageDOAllScheme.GenerateDOAllSchemeReport(entity);
                //Task.Run(() => manageDOAllScheme.GenerateDOAllSchemeReport(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class DeliveryOrderSchemeWiseEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string SchCode { get; set; }
        public string UserID { get; set; }
    }
}
