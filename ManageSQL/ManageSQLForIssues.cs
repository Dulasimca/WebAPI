using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI
{
    public class ManageSQLForIssues
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public bool InsertIssuesEntry(DocumentStockIssuesEntity receiptList)
        {
            SqlTransaction objTrans = null;
            string RowID = string.Empty, SRNo = string.Empty;
            return false;
            //using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
            //{
            //    DataSet ds = new DataSet();

            //    sqlCommand = new SqlCommand();
            //    try
            //    {
            //        if (sqlConnection.State == 0)
            //        {
            //            sqlConnection.Open();
            //        }
            //        objTrans = sqlConnection.BeginTransaction();
            //        sqlCommand.Transaction = objTrans;
            //        sqlCommand.Connection = sqlConnection;
            //        sqlCommand.CommandText = "InsertSRDetails";
            //        sqlCommand.CommandType = CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@SRDate", receiptList.SRDate);
            //        sqlCommand.Parameters.AddWithValue("@Trcode", receiptList.Trcode);
            //        sqlCommand.Parameters.AddWithValue("@Pallotment", receiptList.PAllotment);
            //        sqlCommand.Parameters.AddWithValue("@OrderNo", receiptList.OrderNo);
            //        sqlCommand.Parameters.AddWithValue("@OrderDate", receiptList.OrderDate);
            //        sqlCommand.Parameters.AddWithValue("@ReceivingCode", receiptList.ReceivingCode);
            //        sqlCommand.Parameters.AddWithValue("@IssuingCode", receiptList.DepositorCode);
            //        sqlCommand.Parameters.AddWithValue("@TruckMemoNo", receiptList.TruckMemoNo);
            //        sqlCommand.Parameters.AddWithValue("@TruckMemoDate", receiptList.TruckMemoDate);
            //        sqlCommand.Parameters.AddWithValue("@GServiceable", "0");
            //        sqlCommand.Parameters.AddWithValue("@GUnserviceable", "0");
            //        sqlCommand.Parameters.AddWithValue("@GPatches", "0");
            //        sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
            //        sqlCommand.Parameters.AddWithValue("@IssuerType", receiptList.DepositorType);
            //        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
            //        sqlCommand.Parameters.AddWithValue("@Unloadingslip", "F");
            //        sqlCommand.Parameters.AddWithValue("@Acknowledgementslip", "F");
            //        sqlCommand.Parameters.AddWithValue("@Suspnstack", "-");
            //        sqlCommand.Parameters.AddWithValue("@Suspnseaccno", "-");
            //        sqlCommand.Parameters.AddWithValue("@Gunnyutilised", "0");
            //        sqlCommand.Parameters.AddWithValue("@GunnyReleased", "0");
            //        sqlCommand.Parameters.AddWithValue("@Flag1", "-");
            //        sqlCommand.Parameters.AddWithValue("@Flag2", "-");
            //        sqlCommand.Parameters.AddWithValue("@SRNo1", receiptList.SRNo);
            //        sqlCommand.Parameters.AddWithValue("@RowId1", receiptList.RowId);
            //        sqlCommand.Parameters.Add("@SRNo", SqlDbType.NVarChar, 13);
            //        sqlCommand.Parameters.Add("@RowId", SqlDbType.NVarChar, 30);
            //        sqlCommand.Parameters["@SRNo"].Direction = ParameterDirection.Output;
            //        sqlCommand.Parameters["@RowId"].Direction = ParameterDirection.Output;
            //        sqlCommand.ExecuteNonQuery();

            //        RowID = (string)sqlCommand.Parameters["@RowId"].Value;
            //        SRNo = (string)sqlCommand.Parameters["@SRNO"].Value;
            //        //Delete Sr Item Details
            //        sqlCommand.Parameters.Clear();
            //        sqlCommand.Dispose();

            //        sqlCommand = new SqlCommand();
            //        sqlCommand.Transaction = objTrans;
            //        sqlCommand.Connection = sqlConnection;
            //        sqlCommand.CommandText = "DeleteSRDetails";
            //        sqlCommand.CommandType = CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
            //        sqlCommand.ExecuteNonQuery();

            //        //Insert data into SR Item details

            //        List<DocumentStockIssuesItemEntity> stockReceiptItems = new List<DocumentStockIssuesItemEntity>();
            //        stockReceiptItems = receiptList.itemEntities;
            //        foreach (var item in stockReceiptItems)
            //        {
            //            sqlCommand.Parameters.Clear();
            //            sqlCommand.Dispose();

            //            sqlCommand = new SqlCommand();
            //            sqlCommand.Transaction = objTrans;
            //            sqlCommand.Connection = sqlConnection;
            //            sqlCommand.CommandText = "InsertSRItemDetails";
            //            sqlCommand.CommandType = CommandType.StoredProcedure;
            //            sqlCommand.Parameters.AddWithValue("@RowId", RowID);
            //            sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
            //            sqlCommand.Parameters.AddWithValue("@TStockNo", item.TStockNo);
            //            sqlCommand.Parameters.AddWithValue("@ICode", item.ICode);
            //            sqlCommand.Parameters.AddWithValue("@IPCode", item.IPCode);
            //            sqlCommand.Parameters.AddWithValue("@NoPacking", item.NoPacking);
            //            sqlCommand.Parameters.AddWithValue("@WTCode", item.WTCode);
            //            sqlCommand.Parameters.AddWithValue("@GKgs", item.GKgs);
            //            sqlCommand.Parameters.AddWithValue("@Nkgs", item.Nkgs);
            //            sqlCommand.Parameters.AddWithValue("@Moisture", item.Moisture);
            //            sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
            //           // sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
            //            sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
            //            //sqlCommand.Parameters.AddWithValue("@flag1", "-");
            //            //sqlCommand.Parameters.AddWithValue("@Flag2", "-");
            //            sqlCommand.ExecuteNonQuery();
            //        }

            //        //Insert SRT Details table
            //        sqlCommand.Parameters.Clear();
            //        sqlCommand.Dispose();

            //        sqlCommand = new SqlCommand();
            //        sqlCommand.Transaction = objTrans;
            //        sqlCommand.Connection = sqlConnection;
            //        sqlCommand.CommandText = "InsertSRTDetails";
            //        sqlCommand.CommandType = CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
            //        sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
            //        sqlCommand.Parameters.AddWithValue("@Remarks", receiptList.Remarks);
            //        sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
            //        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
            //        sqlCommand.ExecuteNonQuery();
            //        objTrans.Commit();
            //        return true;

            //    }
            //    catch (Exception ex)
            //    {
            //        AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
            //        objTrans.Rollback();
            //        return false;
            //    }
            //    finally
            //    {
            //        sqlConnection.Close();
            //        sqlCommand.Dispose();
            //        ds.Dispose();
                  
            //    }
            //}
        }
    }
}
