using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNCSCAPI.Controllers.GToG
{
    [Route("api/[controller]")]
    [ApiController]
    public class GToGAckController : ControllerBase
    {
        [HttpPost("{id}")]
        public string Post(GToGParameter gToG)
        {
            bool isUpdated = false;
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", gToG.IssueMemoNumber));
            isUpdated = manageSQLConnection.UpdateValues("UpdateGToGAck", sqlParameters);
            return JsonConvert.SerializeObject(GetMessage(gToG.IssueMemoNumber, isUpdated));
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