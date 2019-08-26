using System;
using System.Data;
using System.Data.SqlClient;
using TNCSCAPI.Controllers.GodownProfile;

namespace TNCSCAPI.ManageSQL
{
    public class ManageGodownProfileSQL
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertGodownProfile(GodownProfileEntity godownProfile)
        {
            string RowID = string.Empty, SINo = string.Empty;
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
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertGodownProfile";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@RowId", godownProfile.RowId);
                    sqlCommand.Parameters.AddWithValue("@GodownCode", godownProfile.GodownCode);
                    sqlCommand.Parameters.AddWithValue("@Gname", godownProfile.Gname);
                    sqlCommand.Parameters.AddWithValue("@desig", godownProfile.desig);
                    sqlCommand.Parameters.AddWithValue("@add1", godownProfile.add1);
                    sqlCommand.Parameters.AddWithValue("@add2", godownProfile.add2);
                    sqlCommand.Parameters.AddWithValue("@add3", godownProfile.add3);
                    sqlCommand.Parameters.AddWithValue("@telno", godownProfile.telno);
                    sqlCommand.Parameters.AddWithValue("@mobno", godownProfile.mobno);
                    sqlCommand.Parameters.AddWithValue("@faxno", godownProfile.faxno);
                    sqlCommand.ExecuteNonQuery();
                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);

                }
                catch (Exception ex)
                {
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
