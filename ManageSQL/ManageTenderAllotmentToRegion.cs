using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TNCSCAPI.Models.Purchase;

namespace TNCSCAPI.ManageSQL
{
    public class ManageTenderAllotmentToRegion
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertRegionTenderAllotmentDetails(TenderAllotmentToRegionEntity entity)
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
                    //foreach (var item in entity)
                    //{
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertRegionalTenderAllotementDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@RegAllotmentID", entity.RegAllotmentID);
                        sqlCommand.Parameters.AddWithValue("@OrderNumber", entity.OrderNumber);
                        sqlCommand.Parameters.AddWithValue("@Quantity", entity.Quantity);
                        sqlCommand.Parameters.AddWithValue("@RCode", entity.RCode);
                        sqlCommand.Parameters.AddWithValue("@Spell", entity.Spell);
                        sqlCommand.Parameters.AddWithValue("@PartyCode", entity.PartyCode);
                        sqlCommand.ExecuteNonQuery();
                   // }
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