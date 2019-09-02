using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageSQL
{
    public class ManageSQLForDDCheque
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string,string> InsertDDChequeEntry(DDChequeEntity chequeEntity)
        {
            SqlTransaction objTrans = null;
            string RowID = string.Empty;
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
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "Insertmemofile";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                  //  sqlCommand.Parameters.AddWithValue("@RowId", chequeEntity.RowId);
                    sqlCommand.Parameters.AddWithValue("@vrno1", chequeEntity.ReceiptNo);
                    sqlCommand.Parameters.AddWithValue("@godcode", chequeEntity.GCode);
                    sqlCommand.Parameters.AddWithValue("@detail", chequeEntity.Details);
                    sqlCommand.Parameters.AddWithValue("@vropt", "1");
                    sqlCommand.Parameters.AddWithValue("@casopt", "CL1");                   
                    sqlCommand.Parameters.AddWithValue("@eflag", "N");                    
                    sqlCommand.Parameters.Add("@vrno", SqlDbType.NVarChar, 26);
                    sqlCommand.Parameters["@vrno"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    RowID = Convert.ToString(sqlCommand.Parameters["@vrno"].Value);

                    foreach (var item in chequeEntity.DDChequeItems)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertReceiptChequedetail";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@vrno", chequeEntity.ReceiptNo);
                        sqlCommand.Parameters.AddWithValue("@godcode", chequeEntity.GCode);
                        sqlCommand.Parameters.AddWithValue("@cdopt", item.PaymentType);
                        sqlCommand.Parameters.AddWithValue("@cdno", item.ChequeNo);
                        sqlCommand.Parameters.AddWithValue("@cddate", item.ChequeDate);
                        sqlCommand.Parameters.AddWithValue("@cdamount", item.Amount);
                        sqlCommand.Parameters.AddWithValue("@cdbank", item.Bank);
                        sqlCommand.Parameters.AddWithValue("@cdwhom", item.ReceivedFrom);
                        sqlCommand.Parameters.AddWithValue("@whomcode", item.ReceivorCode);
                        sqlCommand.Parameters.AddWithValue("@recdate", item.ReceiptDate);
                        sqlCommand.Parameters.AddWithValue("@eflag", "N");
                        sqlCommand.ExecuteNonQuery();
                    }                   
                    return new Tuple<bool, string,string>(true, GlobalVariable.SavedMessage, RowID);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    return new Tuple<bool, string,string>(false, GlobalVariable.ErrorMessage,"");
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
