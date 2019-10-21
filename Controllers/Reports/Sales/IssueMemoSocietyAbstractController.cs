using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageAllReports;
using TNCSCAPI.ManageAllReports.Sales;

namespace TNCSCAPI.Controllers.Reports.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueMemoSocietyAbstractController : ControllerBase
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
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
                ds = manageSQLConnection.GetDataSetValues("GetIssueMemoSocietyAbstract", sqlParameters);
                IssueMemoSocietyAbstract salesIssueMemo = new IssueMemoSocietyAbstract();
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
                        RName= issueMemoDetails.RName,
                        GName= issueMemoDetails.GName
                    };
                    Task.Run(() => salesIssueMemo.GenerateIssueMemoReceipt(entity,GlobalVariable.IssueMemoSocietyAbstractFileName,"IssueMemo Society Abstract",0));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (issueMemoDetails.Type == 2)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
                ds = manageSQLConnection.GetDataSetValues("GetIssueMemoSocietyDateWise", sqlParameters);
                IssueMemoSocietyAbstract salesIssueMemoAbstract = new IssueMemoSocietyAbstract();
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
                        RName = issueMemoDetails.RName,
                        GName = issueMemoDetails.GName
                    };
                    Task.Run(() => salesIssueMemoAbstract.GenerateIssueMemoReceipt(entity, GlobalVariable.IssueMemoSocietyDateWiseFileName, "IssueMemo Society Date wise Abstract", 1));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (issueMemoDetails.Type == 3)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
                ds = manageSQLConnection.GetDataSetValues("GetIssueMemoSocietyDateAndScheme", sqlParameters);
                IssueMemoSocietyAbstract salesIssueMemoAbstract = new IssueMemoSocietyAbstract();
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
                        RName = issueMemoDetails.RName,
                        GName = issueMemoDetails.GName
                    };
                    Task.Run(() => salesIssueMemoAbstract.GenerateIssueMemoReceipt(entity, GlobalVariable.IssueMemoSocietyDateWiseFileName, "IssueMemo Society Date wise Abstract", 1));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (issueMemoDetails.Type == 4)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issueMemoDetails.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@type", issueMemoDetails.ReceivorType));
                sqlParameters.Add(new KeyValuePair<string, string>("@FDATE", issueMemoDetails.Fdate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", issueMemoDetails.Tdate));
                ds = manageSQLConnection.GetDataSetValues("GetIssueMemoSocietySchemeWise", sqlParameters);
                IssueMemoSocietyAbstract salesIssueMemoAbstract = new IssueMemoSocietyAbstract();
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
                        RName = issueMemoDetails.RName,
                        GName = issueMemoDetails.GName
                    };
                    Task.Run(() => salesIssueMemoAbstract.GenerateIssueMemoReceipt(entity, GlobalVariable.IssueMemoSocietyDateWiseFileName, "IssueMemo Society Scheme wise Abstract", 0));
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return JsonConvert.SerializeObject("");
        }
    }
}