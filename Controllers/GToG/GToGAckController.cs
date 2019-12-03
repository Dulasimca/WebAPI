using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.GToG
{
    [Route("api/[controller]")]
    [ApiController]
    public class GToGAckController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(string IssueMemoNumber,string FpsAckDate)
        {
            bool isUpdated = false;
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", IssueMemoNumber));
            isUpdated= manageSQLConnection.UpdateValues("UpdateGToGAck", sqlParameters);
            return JsonConvert.SerializeObject(GetMessage(IssueMemoNumber, isUpdated));
        }

        public GToGEntity GetMessage(string IssueMemoNumber, bool isUpdated)
        {
            try
            {
                GToGEntity gEntity = new GToGEntity();
                gEntity.IssueMemoNumber = IssueMemoNumber;
                if (isUpdated)
                {
                    gEntity.StatusCode = "0";
                    gEntity.Message = "Success";
                }
                else
                {
                    gEntity.StatusCode = "2000";
                    gEntity.Message = "Error";
                }
                return gEntity;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return null;
            }
        }

    }
    public class GToGEntity
    {
        public string IssueMemoNumber { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}