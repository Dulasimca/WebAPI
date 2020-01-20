using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TNCSCAPI.Models;
using System.Data;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageGodownMasterController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string RCode)
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
            //sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
            ds = manageSQLConnection.GetDataSetValues("GetGodownMasterData", sqlParameters);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        [HttpPost("{id}")]
        public bool Post(GodownEntity godownEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@TNCSCode", godownEntity.TNCSCode));
                parameterList.Add(new KeyValuePair<string, string>("@TNCSName", godownEntity.TNCSName));
                parameterList.Add(new KeyValuePair<string, string>("@TNCSRegn", godownEntity.TNCSRegn));
                parameterList.Add(new KeyValuePair<string, string>("@TNCSType", godownEntity.TNCSType));
                parameterList.Add(new KeyValuePair<string, string>("@TNCSCarpet", godownEntity.TNCSCarpet));
                parameterList.Add(new KeyValuePair<string, string>("@TNCSCapacity", godownEntity.TNCSCapacity));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", godownEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@ActiveFlag", godownEntity.ActiveFlag));
                parameterList.Add(new KeyValuePair<string, string>("@SessionFlag", godownEntity.SessionFlag));
                parameterList.Add(new KeyValuePair<string, string>("@ExportFlag", godownEntity.ExportFlag));
                parameterList.Add(new KeyValuePair<string, string>("@OPERATIONTYPE", godownEntity.OPERATIONTYPE));
                parameterList.Add(new KeyValuePair<string, string>("@NOOFSHOPCRS", Convert.ToString(godownEntity.NOOFSHOPCRS)));
                parameterList.Add(new KeyValuePair<string, string>("@DocStatus", Convert.ToString(godownEntity.DocStatus)));
                parameterList.Add(new KeyValuePair<string, string>("@CBStatement", Convert.ToString(godownEntity.CBStatement)));
                return manageSQLConnection.InsertData("InsertGodownMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class GodownEntity
    {
        public string TNCSCode { get; set; }
        public string TNCSName { get; set; }
        public string TNCSRegn { get; set; }
        public string TNCSType { get; set; }
        public string TNCSCarpet { get; set; }
        public string TNCSCapacity { get; set; }
        public string DeleteFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string SessionFlag { get; set; }
        public string ExportFlag { get; set; }
        public string OPERATIONTYPE { get; set; }
        public int NOOFSHOPCRS { get; set; }
        public bool DocStatus { get; set; }
        public bool CBStatement { get; set; }
    }
}