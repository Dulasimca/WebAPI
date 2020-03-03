using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TNCSCAPI.Controllers.Reports.GtoG;

namespace TNCSCAPI.Controllers.GToG
{
    [Route("api/[controller]")]
    [ApiController]
    public class GToGAckController : ControllerBase
    {
        [HttpPost("{id}")]
        public GToGEntity Post(GToGParameter gtog)
        {
            bool isUpdated = false;
            if (!string.IsNullOrEmpty(gtog.IssueMemoNumber))
            {
                ManageGtoG manageGtoG = new ManageGtoG();
                AuditLog.WriteError("IssueMemoNumber is : " + gtog.IssueMemoNumber);
                //Check issuememo number
                var result = manageGtoG.CheckIssueMemoNumber(gtog.IssueMemoNumber);
                if (result.Item1)
                {
                    ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", gtog.IssueMemoNumber));
                    isUpdated = manageSQLConnection.UpdateValues("UpdateGToGAck", sqlParameters);
                    return GetMessage(gtog.IssueMemoNumber, isUpdated);
                }
                else
                {
                    return result.Item2;
                }
            }
            else
            {
                AuditLog.WriteError("IssueMemoNumber is : " + gtog.IssueMemoNumber);
            }

            return GetNullIssuememo(gtog.IssueMemoNumber);
        }

        [HttpGet("{id}")]
        public string Get(string IssueMemoNumber)
        {
            bool isUpdated = false;
            if (!string.IsNullOrEmpty(IssueMemoNumber) && IssueMemoNumber.Length > 3)
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@DocNumber", IssueMemoNumber));
                isUpdated = manageSQLConnection.UpdateValues("UpdateGToGAck", sqlParameters);
            }
            else
            {
                isUpdated = false;
            }
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

        public GToGEntity GetNullIssuememo(string IssueMemoNumber)
        {
            try
            {
                GToGEntity gEntity = new GToGEntity();
                gEntity.IssueMemoNumber = IssueMemoNumber;
                gEntity.StatusCode = "2000";
                gEntity.Message = "Issuememo number is null, please check your parameter";
                return gEntity;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return null;
            }
        }
    }



}