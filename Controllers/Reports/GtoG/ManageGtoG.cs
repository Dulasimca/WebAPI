using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace TNCSCAPI.Controllers.Reports.GtoG
{
    public class ManageGtoG
    {
        public Tuple<bool,GToGEntity> CheckIssueMemoNumber(string IssueMomo)
        {
            GToGEntity gEntity = new GToGEntity();
            bool isAvailable = false;
            try
            {
                gEntity.IssueMemoNumber = IssueMomo;
                DataSet dataset = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", IssueMomo));
                dataset= manageSQLConnection.GetDataSetValues("CheckIssueMomo", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(dataset))
                {
                    isAvailable = true;
                    gEntity.StatusCode = "0";
                    gEntity.Message = "Success";
                }
                else
                {
                    isAvailable = false;
                    gEntity.StatusCode = "2000";
                    gEntity.Message = "Issuemomo Number is not valid or Already ACK has been updated.";
                }
                return new Tuple<bool, GToGEntity>(isAvailable, gEntity);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                gEntity.StatusCode = "2000";
                gEntity.Message = "Internal Error";
                gEntity.IssueMemoNumber = IssueMomo;
                return new Tuple<bool, GToGEntity>(false, gEntity);
            }
        }

    }
    public class GToGParameter
    {
        public string IssueMemoNumber { get; set; }
        public string FpsAckDate { get; set; }
    }
    public class GToGEntity
    {
        public string IssueMemoNumber { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}
