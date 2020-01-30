﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuerMasterController : ControllerBase
    {

         [HttpGet("{id}")]
        public string Get(string GCode, int Type = 0)
        {
            if(Type == 1)
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuerPartyMaster", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else if(Type==2)
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuersMasterAllData", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetIssuerMaster", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
         
        }

        [HttpPut("{id}")]
        public bool Put(IssuerEntity issuerEntity)
        {
            if (issuerEntity.Type == 1)
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", issuerEntity.IssuerCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@PartyID", issuerEntity.PartyID));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", issuerEntity.RCode));
                return manageSQLConnection.UpdateValues("UpdateIssuerPartyMaster", sqlParameters);
            }
            else
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", issuerEntity.IssuerCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Activeflag", issuerEntity.Activeflag));
                sqlParameters.Add(new KeyValuePair<string, string>("@ACSCode", issuerEntity.ACSCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Godcode", issuerEntity.GCode));
                return manageSQLConnection.UpdateValues("UpdateIssuerMaster", sqlParameters);
            }
        }

        [HttpPost("{id}")]
        public bool Post(IssuerEntity issuerEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerNo", issuerEntity.IssuerNo));
            sqlParameters.Add(new KeyValuePair<string, string>("@Issuername", issuerEntity.IssuerName));
            sqlParameters.Add(new KeyValuePair<string, string>("@SocietyCode", issuerEntity.SocietyCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", issuerEntity.IssuerCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@CategoryId", issuerEntity.CategoryId));
            sqlParameters.Add(new KeyValuePair<string, string>("@Tycode", issuerEntity.Tycode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Beneficiaries", issuerEntity.NoOfBeneficiaries));
            sqlParameters.Add(new KeyValuePair<string, string>("@Activeflag", issuerEntity.Activeflag));
            sqlParameters.Add(new KeyValuePair<string, string>("@ACSCode", issuerEntity.ACSCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", issuerEntity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@RCode", issuerEntity.RCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GSTNo", issuerEntity.GSTNumber));
            return manageSQLConnection.InsertData("InsertIssuerMaster", sqlParameters);
        }


    }
    public class IssuerEntity
    {
        public int Type { get; set; }
        public string PartyID { get; set; }
        public string IssuerNo { get; set; }
        public string IssuerName { get; set; }
        public string IssuerCode { get; set; }
        public string Activeflag { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string ACSCode { get; set; }
        public string SocietyCode { get; set; }
        public string CategoryId { get; set; }
        public string Tycode { get; set; }
        public string NoOfBeneficiaries { get; set; }
        public string GSTNumber { get; set; }
    }
}