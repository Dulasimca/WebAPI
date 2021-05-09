using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.HOqtyCon.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferOBToHoQtyController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode, string CurYear)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@CurYear", CurYear));
            ds = manageSQLConnection.GetDataSetValues("GetHOQtyRegionWise", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPost("{id}")]
        public Tuple<bool, string> Post([FromBody] List<TransferOBToHOQtyEntity> entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            try
            {
                foreach (var item in entity)
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", item.RegionCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@QtyMonth", item.QTYMONTH));
                    sqlParameters.Add(new KeyValuePair<string, string>("@QtyYear", item.QTYYEAR));
                    sqlParameters.Add(new KeyValuePair<string, string>("@ITCode", item.CommodityCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Location", item.Location));
                    sqlParameters.Add(new KeyValuePair<string, string>("@OB", item.OB));
                    sqlParameters.Add(new KeyValuePair<string, string>("@PB", item.PB));
                    sqlParameters.Add(new KeyValuePair<string, string>("@CS", item.CS));
                    manageSQLConnection.InsertData("InsertOBIntoHOQtyAbstract", sqlParameters);
                }
                return new Tuple<bool, string> (true, GlobalVariable.SavedMessage);
            }
            catch (Exception ex)
            {

                AuditLog.WriteError("Allotment" + ex.Message + " : " + ex.StackTrace);
                return new Tuple<bool, string>(false, GlobalVariable.ErrorMessage);
            }

        }
    }

    public class TransferOBToHOQtyEntity
    {
        public string RegionCode { get; set; }
        public string QTYMONTH { get; set; }
        public string QTYYEAR { get; set; }
        public string Location { get; set; }
        public string OB { get; set; }
        public string PB { get; set; }
        public string CS { get; set; }
        public string CommodityCode { get; set; }
    }

}
