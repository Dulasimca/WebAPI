﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationDetailsController : ControllerBase
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();

        [HttpPost("{id}")]
        public Tuple<bool, string> Post(QuotationEntity entity)
        {
            SqlTransaction objTrans = null;
            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
            {
                DataSet ds = new DataSet();

                sqlCommand = new SqlCommand();
                try
                {
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    objTrans = sqlConnection.BeginTransaction();
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertQuotationDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ProductID", entity.ProductID);
                    sqlCommand.Parameters.AddWithValue("@EmailID", entity.EmailID);
                    sqlCommand.Parameters.AddWithValue("@PhoneNo", entity.PhoneNo);
                    sqlCommand.Parameters.AddWithValue("@Remarks", entity.Remarks);
                    sqlCommand.Parameters.AddWithValue("@GCode", entity.GCode);
                    sqlCommand.Parameters.AddWithValue("@RCode", entity.RCode);
                    sqlCommand.ExecuteNonQuery();

                    objTrans.Commit();
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();
                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
                }
                catch (Exception ex)
                {
                    objTrans.Rollback();
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    return new Tuple<bool, string>(false, GlobalVariable.ErrorMessage);
                }
                finally
                {
                    sqlConnection.Close();
                    sqlCommand.Dispose();
                    ds.Dispose();
                }
            }
        }
    }
}
