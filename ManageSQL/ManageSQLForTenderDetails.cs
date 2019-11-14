using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.ManageSQL
{
    public class ManageSQLForTenderDetails
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertTenderDetails(TenderDetailsEntity entity)
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
                    //sqlCommand.Transaction = objTrans;
                    //sqlCommand.Connection = sqlConnection;
                            sqlCommand.Parameters.Clear();
                            sqlCommand.Dispose();

                            sqlCommand = new SqlCommand();
                            sqlCommand.Transaction = objTrans;
                            sqlCommand.Connection = sqlConnection;
                    if(entity.Type == 2)
                    {
                        sqlCommand.CommandText = "InsertTenderQuantityDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Quantity", entity.AdditionalQty);
                        sqlCommand.Parameters.AddWithValue("@OrderNumber", entity.OrderNumber);
                    }
                    else
                    {
                            sqlCommand.CommandText = "InsertTenderDetails";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@TenderDetID", entity.TenderDetId);
                            sqlCommand.Parameters.AddWithValue("@TenderId", entity.TenderId);
                            sqlCommand.Parameters.AddWithValue("@OrderNumber", entity.OrderNumber);
                            sqlCommand.Parameters.AddWithValue("@OrderDate", entity.OrderDate);
                            sqlCommand.Parameters.AddWithValue("@TenderDate", entity.TenderDate);
                            sqlCommand.Parameters.AddWithValue("@CompletedDate", entity.CompletedDate);
                            sqlCommand.Parameters.AddWithValue("@ITCode", entity.ITCode);
                            sqlCommand.Parameters.AddWithValue("@Quantity", entity.Quantity);
                            sqlCommand.Parameters.AddWithValue("@Remarks", entity.Remarks);
                    }

                    sqlCommand.ExecuteNonQuery();
                    objTrans.Commit();
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
   
