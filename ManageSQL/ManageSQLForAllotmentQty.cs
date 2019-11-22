using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Controllers.Allotment;

namespace TNCSCAPI.ManageSQL
{
    public class ManageSQLForAllotmentQty
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertAllotmentQtyEntry(List<AllotmentQuantityEntity> entity)
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
                    foreach (var item in entity)
                    {
                        foreach (var i in item.ItemList)
                        {
                            sqlCommand.Parameters.Clear();
                            sqlCommand.Dispose();

                            sqlCommand = new SqlCommand();
                            sqlCommand.Transaction = objTrans;
                            sqlCommand.Connection = sqlConnection;
                            sqlCommand.CommandText = "InsertAllotmentQuantity";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@SocietyCode", item.SocietyCode);
                            sqlCommand.Parameters.AddWithValue("@FPSCode", item.FPSCode);
                            sqlCommand.Parameters.AddWithValue("@SchemeCode", '-');
                            sqlCommand.Parameters.AddWithValue("@AMonth", item.AllotmentMonth);
                            sqlCommand.Parameters.AddWithValue("@AYear", item.AllotmentYear);
                            sqlCommand.Parameters.AddWithValue("@GCode", item.GCode);
                            sqlCommand.Parameters.AddWithValue("@Taluk", item.Taluk);

                            sqlCommand.Parameters.AddWithValue("@ITCode", i.ITCode);
                            sqlCommand.Parameters.AddWithValue("@Quantity", i.Quantity);

                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    objTrans.Commit();
                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
                }
                catch (Exception ex)
                {
                    objTrans.Rollback();
                    AuditLog.WriteError("Allotment"+ ex.Message + " : " + ex.StackTrace);
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
