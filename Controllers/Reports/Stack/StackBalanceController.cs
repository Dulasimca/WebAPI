using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using TNCSCAPI.ManageAllReports.Stack;

namespace TNCSCAPI.Controllers.Reports.Stack
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackBalanceController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(StackEntity stackEntity)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            if (stackEntity.Type == 1)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stackEntity.ICode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stackEntity.StackDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@StacKNo", stackEntity.TStockNo));
                sqlParameters.Add(new KeyValuePair<string, string>("@DocNo", stackEntity.DocNo));
                ds = manageSQLConnection.GetDataSetValues("GetStackBalance", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (stackEntity.Type == 2)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ICode", stackEntity.ICode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Fyear", stackEntity.StackDate));
                ds = manageSQLConnection.GetDataSetValues("GetStackcardOpenentry", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (stackEntity.Type == 3)
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ICode", stackEntity.ICode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Fyear", stackEntity.StackDate));
                ds = manageSQLConnection.GetDataSetValues("GetStackcard", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if (stackEntity.Type == 4)
            {
                ManageStackCard manageStackCard = new ManageStackCard();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stackEntity.ICode));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stackEntity.StackDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@StacKNo", stackEntity.TStockNo.Trim()));
                sqlParameters.Add(new KeyValuePair<string, string>("@StackYear", stackEntity.StackYear));
                ds = manageSQLConnection.GetDataSetValues("GetStackCardDetailsbyId", sqlParameters);
                //Calculate the 
                var result= manageStackCard.ManageStackBalance(ds, stackEntity);
                return JsonConvert.SerializeObject(result);
            }
            else if (stackEntity.Type == 5)
            {
                ManageStackCard manageStackCard = new ManageStackCard();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", stackEntity.GCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ItemCode", stackEntity.ICode));
                sqlParameters.Add(new KeyValuePair<string, string>("@ShortYear", stackEntity.StackDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@Status", stackEntity.StackStatus));
                sqlParameters.Add(new KeyValuePair<string, string>("@FromDate", stackEntity.FromDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@ToDate", stackEntity.ToDate));
                ds = manageSQLConnection.GetDataSetValues("GetStackCardDetailsbyCommodity", sqlParameters);
                //Calculate the 
                var result = manageStackCard.ManageStackCardRegister(ds, stackEntity);
                return JsonConvert.SerializeObject(result);
            }
            return string.Empty;
        }

        [HttpDelete("{id}")]
        public bool Delete(string RowId, string GCode)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RowId", RowId));
            return manageSQLConnection.UpdateValues("DeleteStackDetails", sqlParameters);
            //Calculate the 
        }
    }
    public class StackEntity
    {
        public string GCode { get; set; }
        public string ICode { get; set; }
        public string StackDate { get; set; }
        public string TStockNo { get; set; }
        public string StackStatus { get; set; }
        public int Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StackYear { get; set; }

        public string GName { get; set; }
        public string RName { get; set; }
        public string ITName { get; set; }
        public string UserName { get; set; }
        public string DocNo { get; set; }

    }
}