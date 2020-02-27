using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Controllers.Audit_Inception;

namespace TNCSCAPI.ManageSQL
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSQLForInception
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertInceptionDetails(AuditInceptionEntity entity)
        {
            SqlTransaction objTrans = null;
            int InceptionID;
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
                    sqlCommand.CommandText = "InsertInceptionDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@InceptionTeam", entity.InceptionTeam);
                    sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                    sqlCommand.Parameters.AddWithValue("@Designation", entity.Designation);
                    sqlCommand.Parameters.AddWithValue("@InceptionDate", entity.InceptionDate);
                    sqlCommand.Parameters.AddWithValue("@Remarks", entity.Remarks);
                    sqlCommand.Parameters.AddWithValue("@GCode", entity.GCode);
                    sqlCommand.Parameters.AddWithValue("@InceptionID1", entity.InceptionID);
                    sqlCommand.Parameters.Add("@InceptionID", SqlDbType.Int, 30);
                    sqlCommand.Parameters["@InceptionID"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    InceptionID = Convert.ToInt32(sqlCommand.Parameters["@InceptionID"].Value);
                    foreach (var i in entity.InceptionData)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertInceptionItemDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@InceptionID", InceptionID);
                        sqlCommand.Parameters.AddWithValue("@InceptionItemID", i.InceptionItemID);
                        sqlCommand.Parameters.AddWithValue("@ITCode", i.ITCode);
                        sqlCommand.Parameters.AddWithValue("@StackNoRowID", i.StackRowId);
                        sqlCommand.Parameters.AddWithValue("@TypeCode", i.TypeCode);
                        sqlCommand.Parameters.AddWithValue("@Quantity", i.Quantity);
                        sqlCommand.Parameters.AddWithValue("@CurYear", i.CurrYear);
                        sqlCommand.ExecuteNonQuery();
                    }
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