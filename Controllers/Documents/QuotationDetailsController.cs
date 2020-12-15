using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
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
            string QId = string.Empty;
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
                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertQuotationDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@EmailID", entity.EmailID);
                    sqlCommand.Parameters.AddWithValue("@PhoneNo", entity.PhoneNo);
                    sqlCommand.Parameters.AddWithValue("@Remarks", entity.Remarks);
                    sqlCommand.Parameters.AddWithValue("@GCode", entity.GCode);
                    sqlCommand.Parameters.AddWithValue("@RCode", entity.RCode);
                    sqlCommand.Parameters.Add("@Q_ID", SqlDbType.Int, 8);
                    sqlCommand.Parameters["@Q_ID"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    QId = Convert.ToString(sqlCommand.Parameters["@Q_ID"].Value);

                    foreach (var item in entity.ProductID)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertQuotationProductDeatils";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Q_Id", QId);
                        sqlCommand.Parameters.AddWithValue("@P_Id", item);
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
