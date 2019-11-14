using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.ManageSQL
{
    public class ManageSQLForTenderAllotmentToRegion
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertTenderAllotmentToRegional(TenderAllotmentToRegionEntity entity)
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
                    sqlCommand.CommandText = "InsertRegionalTenderAllotementDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@RegAllotmentID", entity.RegAllotmentID);
                    sqlCommand.Parameters.AddWithValue("@TenderAllotmentID", entity.TenderAllotmentID);
                    sqlCommand.Parameters.AddWithValue("@RCode", entity.RCode);
                    sqlCommand.Parameters.AddWithValue("@Quantity", entity.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Remarks", entity.Remarks);

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

