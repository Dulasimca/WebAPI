using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TNCSCAPI.ManageSQL
{
    public class ManageGatePass
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public void InsertGatePass(GatePassEntity gatePass)
        {
            SqlTransaction objTrans = null;
            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
            {
                if (sqlConnection.State == 0)
                {
                    sqlConnection.Open();
                }
                try
                {
                    sqlCommand = new SqlCommand();
                    sqlCommand = new SqlCommand();
                    objTrans = sqlConnection.BeginTransaction();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertGatePass";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@GatePassId", "0");
                    sqlCommand.Parameters.AddWithValue("@LorryNumber", gatePass.LorryNumber);
                    sqlCommand.Parameters.AddWithValue("@PrintFlag", "0");
                    sqlCommand.Parameters.AddWithValue("@DocumentTye", gatePass.DocumentTye);
                    sqlCommand.Parameters.AddWithValue("@GCode", gatePass.GCode);
                    sqlCommand.Parameters.AddWithValue("@RCode", gatePass.RCode);
                    sqlCommand.Parameters.AddWithValue("@DocumentNumber", gatePass.DocumentNumber);
                    sqlCommand.ExecuteNonQuery();
                    objTrans.Commit();
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                }
            }

        }
    }

    public class GatePassEntity
    {
        public string LorryNumber { get; set; }
        public int DocumentTye { get; set; }
        public string GCode { get; set; }
        public string RCode { get; set; }
        public string DocumentNumber { get; set; }
    }
}
