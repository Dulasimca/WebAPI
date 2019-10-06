using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.DeliveryOrder;

namespace TNCSCAPI.Controllers.Reports.DeliveryOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOSocietyAbstractController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(DeliveryOrderSocietyAbstractEntity societyAbstract)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", societyAbstract.FromDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", societyAbstract.ToDate));
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", societyAbstract.GCode));
            ds = manageSQLConnection.GetDataSetValues("GetDeliveryOrdeSocietyAbstract", sqlParameters);
            ManageDOSociety manageDOSociety = new ManageDOSociety();
            ManageReport manageReport = new ManageReport();
            if (manageReport.CheckDataAvailable(ds))
            {
                CommonEntity entity = new CommonEntity
                {
                    dataSet = ds,
                    GCode = societyAbstract.GCode,
                    FromDate = societyAbstract.FromDate,
                    Todate = societyAbstract.ToDate,
                    UserName = societyAbstract.UserName,
                    GName = societyAbstract.GName,
                    RName = societyAbstract.RName
                };
                manageDOSociety.GenerateDOSocietyScheme(entity);
              //  Task.Run(() => manageDOSociety.GenerateDOSocietyScheme(entity)); //Generate the Report
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
    }
    public class DeliveryOrderSocietyAbstractEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GCode { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public string UserName { get; set; }
    }
}