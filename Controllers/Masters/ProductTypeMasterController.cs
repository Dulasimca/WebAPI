﻿using System;
using System.Collections.Generic;
using System.Data;
<<<<<<< HEAD
=======
using System.Data.SqlClient;
>>>>>>> a94bd6a072f2b6e125126dec07fb88152b333089
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeMasterController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            DataSet ds = new DataSet();
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            ds = manageSQLConnection.GetDataSetValues("GetProductTypeMaster");
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
//<<<<<<< HEAD
//=======

//        [HttpPost("{id}")]
//        public Tuple<bool, string> Post(string PName)
//        {
//            SqlTransaction objTrans = null;
//            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
//            {
//                DataSet ds = new DataSet();

//                sqlCommand = new SqlCommand();
//                try
//                {
//                    if (sqlConnection.State == 0)
//                    {
//                        sqlConnection.Open();
//                    }
//                    objTrans = sqlConnection.BeginTransaction();
//                    sqlCommand.Parameters.Clear();
//                    sqlCommand.Dispose();

//                    sqlCommand = new SqlCommand();
//                    sqlCommand.Transaction = objTrans;
//                    sqlCommand.Connection = sqlConnection;
//                    sqlCommand.CommandText = "InsertQuotationDetails";
//                    sqlCommand.CommandType = CommandType.StoredProcedure;
//                    sqlCommand.Parameters.AddWithValue("@PName", PName);
//                    sqlCommand.ExecuteNonQuery();

//                    objTrans.Commit();
//                    sqlCommand.Parameters.Clear();
//                    sqlCommand.Dispose();
//                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
//                }
//                catch (Exception ex)
//                {
//                    objTrans.Rollback();
//                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
//                    return new Tuple<bool, string>(false, GlobalVariable.ErrorMessage);
//                }
//                finally
//                {
//                    sqlConnection.Close();
//                    sqlCommand.Dispose();
//                    ds.Dispose();
//                }
//            }
//        }
//>>>>>>> a94bd6a072f2b6e125126dec07fb88152b333089
    }
}