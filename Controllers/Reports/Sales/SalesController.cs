using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.Sales;

namespace TNCSCAPI.Controllers.Reports.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(IssueMemoEntity issueMemoDetails)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (issueMemoDetails.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@society", issueMemoDetails.SCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ShopCode", issueMemoDetails.ShopCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate)); 
                ds = manageSQLConnection.GetDataSetValues("Getissuememo", sqlParameters);
                SalesIssueMemo salesIssueMemo = new SalesIssueMemo();
                ManageReport manageReport = new ManageReport();
                //DataTable dt = new DataTable();
                if (manageReport.CheckDataAvailable(ds))
                {
                    CommonEntity entity = new CommonEntity
                    {
                        dataSet = ds,
                        GCode = issueMemoDetails.GCode,
                        FromDate = issueMemoDetails.Fdate,
                        Todate = issueMemoDetails.Tdate,
                        UserName = issueMemoDetails.UserName,
                    };
                    Task.Run(() => salesIssueMemo.GenerateCustomerDetail(entity));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (issueMemoDetails.Type == 2)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@society", issueMemoDetails.SCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ShopCode", issueMemoDetails.ShopCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuememoAbstract", sqlParameters);
                SalesIssueMemoAbstract salesIssueMemoAbstract = new SalesIssueMemoAbstract();
                ManageReport manageReport = new ManageReport();
                //DataTable dt = new DataTable();
                if (manageReport.CheckDataAvailable(ds))
                {
                    CommonEntity entity = new CommonEntity
                    {
                        dataSet = ds,
                        GCode = issueMemoDetails.GCode,
                        FromDate = issueMemoDetails.Fdate,
                        Todate = issueMemoDetails.Tdate,
                        UserName = issueMemoDetails.UserName,
                    };
                    Task.Run(() => salesIssueMemoAbstract.GenerateSalesIssueMemoAbstract(entity));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return JsonConvert.SerializeObject("");
        }
    }

    public class IssueMemoEntity
    {
        public string GCode { get; set; }
        public string SCode { get; set; }
        public string ReceivorType { get; set; }
        public string Fdate { get; set; }
        public string Tdate { get; set; }
        public string UserName { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public int Type { get; set; }
       public string ShopCode { get; set; }
    }
}