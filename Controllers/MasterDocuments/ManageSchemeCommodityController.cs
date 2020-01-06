﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSchemeCommodityController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(SchemesCommodityEntity masterEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@ActiveFlag", masterEntity.ActiveFlag));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", masterEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@SCCode", masterEntity.SCCode));
                parameterList.Add(new KeyValuePair<string, string>("@CCode", masterEntity.CCode));
                parameterList.Add(new KeyValuePair<string, string>("@RowId", Convert.ToString(masterEntity.RowId)));
                return manageSQLConnection.InsertData("InsertSchemeCommodity", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class SchemesCommodityEntity
    {
        public int RowId         {get;set;}
        public string CCode		    {get;set;}
	    public string SCCode	    {get;set;}
	    public string DeleteFlag    {get;set;}
	    public string ActiveFlag { get; set; }
    }
}