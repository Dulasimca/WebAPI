﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using TNCSCAPI.Models;

namespace TNCSCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardCBController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string Date)
        {
            ManageDashboard manageDashboard = new ManageDashboard();
            List<object> list = new List<object>();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                
                ds = manageSQLConnection.GetDataSetValues("DashBordCBGraph");
                if (ds.Tables.Count > 1)
                {
                    string[] regionInfo = ds.Tables[1].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    #region Get's the Rice information
                    string[] riceInfo = ds.Tables[2].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    list.Add(riceInfo);
                    list.Add(regionInfo);
                    foreach (var rice in riceInfo)
                    {
                        list.Add(manageDashboard.GetValueInArray(rice, ds.Tables[0], regionInfo));
                    }
                    #endregion

                    #region Get's the Dhall and oil information
                    string[] dhallInfo = ds.Tables[4].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    foreach (var dhall in dhallInfo)
                    {
                        list.Add(manageDashboard.GetValueInArray(dhall, ds.Tables[3], regionInfo));
                    }
                    #endregion

                    #region Get's wheat and sugar information
                    string[] wheatInfo = ds.Tables[6].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    foreach (var mywheat in wheatInfo)
                    {
                        list.Add(manageDashboard.GetValueInArray(mywheat, ds.Tables[5], regionInfo));
                    }
                    #endregion
                }
            }
            finally
            {
                ds = null;
            }
            return JsonConvert.SerializeObject(list);
        }
    }
}