using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TNCSCAPI.ManageSQL;
using TNCSCAPI.Models.DoToSales;


namespace TNCSCAPI.Controllers.GST.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOrderToSalesTaxController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string fromDate, string toDate , string GCode, int type, int SPType, string BillNo)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            //sqlParameters.Add(new KeyValuePair<string, string>("@month", Month));
            //sqlParameters.Add(new KeyValuePair<string, string>("@year", Year));
            sqlParameters.Add(new KeyValuePair<string, string>("@fromDate", fromDate.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@todate", toDate.ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@BillNo", BillNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@Type", SPType.ToString()));
            if (type == 1)
            {
                ds = manageSQLConnection.GetDataSetValues("GetDODetailsOfGSTSalesTax", sqlParameters);
            }
            else
            {
                sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetDoGSTSales", sqlParameters);
            }
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

    [HttpPost("{id}")]
    public Tuple<bool, string> Post([FromBody]List<DOSalesTaxEntity> entity)
    {
        ManageDOToSalesTax manageSQL = new ManageDOToSalesTax();
        return manageSQL.InsertDoToSalesTax(entity);
    }
   }

}