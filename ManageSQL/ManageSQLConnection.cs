using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI
{
    public class ManageSQLConnection
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();

        SqlDataAdapter dataAdapter;
        /// <summary>
        /// Gets values from 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public DataSet GetDataSetValues(string procedureName)
        {
            sqlConnection = new SqlConnection(GlobalVariable.ConnectionString);
            DataSet ds = new DataSet();
            sqlCommand = new SqlCommand();
            try
            {
                if (sqlConnection.State == 0)
                {
                    sqlConnection.Open();
                }
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(ds);
                return ds;
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }

        }

        public DataSet GetDataSetValues(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            sqlConnection = new SqlConnection(GlobalVariable.ConnectionString);
            DataSet ds = new DataSet();
            sqlCommand = new SqlCommand();
            try
            {
                if (sqlConnection.State == 0)
                {
                    sqlConnection.Open();
                }
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }

                dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        public bool UpdateValues(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            sqlConnection = new SqlConnection(GlobalVariable.ConnectionString);
            DataSet ds = new DataSet();
            sqlCommand = new SqlCommand();
            try
            {
                if (sqlConnection.State == 0)
                {
                    sqlConnection.Open();
                }
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }

                dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(ds);
                return true;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                return false;

            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        public bool InsertData(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            sqlConnection = new SqlConnection(GlobalVariable.ConnectionString);
            DataSet ds = new DataSet();
            sqlCommand = new SqlCommand();
            try
            {
                if (sqlConnection.State == 0)
                {
                    sqlConnection.Open();
                }
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
                sqlCommand.ExecuteNonQuery();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        public Tuple<bool, string> InsertReceiptSrDetailEntry(DocumentStockReceiptList receiptList)
        {
            SqlTransaction objTrans = null;
            string RowID = string.Empty, SRNo = string.Empty;
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
                    sqlCommand.CommandText = "InsertSRDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SRDate", receiptList.SRDate);
                    sqlCommand.Parameters.AddWithValue("@Trcode", receiptList.Trcode);
                    sqlCommand.Parameters.AddWithValue("@Pallotment", receiptList.PAllotment);
                    sqlCommand.Parameters.AddWithValue("@OrderNo", receiptList.OrderNo);
                    sqlCommand.Parameters.AddWithValue("@OrderDate", receiptList.OrderDate);
                    sqlCommand.Parameters.AddWithValue("@ReceivingCode", receiptList.ReceivingCode);
                    sqlCommand.Parameters.AddWithValue("@IssuingCode", receiptList.DepositorCode);
                    sqlCommand.Parameters.AddWithValue("@TruckMemoNo", receiptList.TruckMemoNo);
                    sqlCommand.Parameters.AddWithValue("@TruckMemoDate", receiptList.TruckMemoDate);
                    sqlCommand.Parameters.AddWithValue("@GServiceable", "0");
                    sqlCommand.Parameters.AddWithValue("@GUnserviceable", "0");
                    sqlCommand.Parameters.AddWithValue("@GPatches", "0");
                    sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
                    sqlCommand.Parameters.AddWithValue("@IssuerType", receiptList.DepositorType);
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@Unloadingslip", receiptList.UnLoadingSlip);
                    sqlCommand.Parameters.AddWithValue("@Acknowledgementslip", "F");
                    sqlCommand.Parameters.AddWithValue("@Suspnstack", "-");
                    sqlCommand.Parameters.AddWithValue("@Suspnseaccno", "-");
                    sqlCommand.Parameters.AddWithValue("@Gunnyutilised", "0");
                    sqlCommand.Parameters.AddWithValue("@GunnyReleased", "0");
                    sqlCommand.Parameters.AddWithValue("@Flag1", "-");
                    sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                    sqlCommand.Parameters.AddWithValue("@SRNo1", receiptList.SRNo);
                    sqlCommand.Parameters.AddWithValue("@RowId1", receiptList.RowId);
                    sqlCommand.Parameters.Add("@SRNo", SqlDbType.NVarChar, 13);
                    sqlCommand.Parameters.Add("@RowId", SqlDbType.NVarChar, 30);
                    sqlCommand.Parameters["@SRNo"].Direction = ParameterDirection.Output;
                    sqlCommand.Parameters["@RowId"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    RowID = (string)sqlCommand.Parameters["@RowId"].Value;
                    SRNo = (string)sqlCommand.Parameters["@SRNO"].Value;

                    //Generate the report file.
                    receiptList.SRNo = SRNo;
                    ManageDocumentReceipt documentReceipt = new ManageDocumentReceipt();
                    Task.Run(() => documentReceipt.GenerateReceipt(receiptList));

                    //Delete Sr Item Details
                    //sqlCommand.Parameters.Clear();
                    //sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "DeleteSRDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
                    sqlCommand.ExecuteNonQuery();

                    ////Insert data into SR Item details

                    List<StockReceiptItemList> stockReceiptItems = new List<StockReceiptItemList>();
                    stockReceiptItems = receiptList.ItemList;
                    foreach (var item in stockReceiptItems)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertSRItemDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
                        sqlCommand.Parameters.AddWithValue("@TStockNo", item.TStockNo);
                        sqlCommand.Parameters.AddWithValue("@ICode", item.ICode);
                        sqlCommand.Parameters.AddWithValue("@IPCode", item.IPCode);
                        sqlCommand.Parameters.AddWithValue("@NoPacking", item.NoPacking);
                        sqlCommand.Parameters.AddWithValue("@WTCode", item.WTCode);
                        sqlCommand.Parameters.AddWithValue("@GKgs", item.GKgs);
                        sqlCommand.Parameters.AddWithValue("@Nkgs", item.Nkgs);
                        sqlCommand.Parameters.AddWithValue("@Moisture", item.Moisture);
                        sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
                        sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        //sqlCommand.Parameters.AddWithValue("@flag1", "-");
                        //sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }

                    ////Insert SRT Details table
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertSRTDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                    sqlCommand.Parameters.AddWithValue("@SRNo", SRNo);
                    sqlCommand.Parameters.AddWithValue("@Remarks", receiptList.Remarks);
                    sqlCommand.Parameters.AddWithValue("@RCode", receiptList.RCode);
                    sqlCommand.Parameters.AddWithValue("@LNo", receiptList.LNo);
                    sqlCommand.Parameters.AddWithValue("@TransportMode", receiptList.MTransport);
                    sqlCommand.Parameters.AddWithValue("@LFrom", receiptList.LFrom);
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.ExecuteNonQuery();
                    objTrans.Commit();
                    return new Tuple<bool, string>(true, SRNo);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string>(false, "Please Contact Administrator");
                }
                finally
                {
                    sqlConnection.Close();
                    sqlCommand.Dispose();
                    ds.Dispose();
                    dataAdapter = null;
                }
            }

        }

        public bool InsertOpeningBalanceMaster(OpeningBalanceEntity openingBalanceList)
        {
            // string Rowid = string.Empty;
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
                    sqlCommand.CommandText = "InsertOpeningBalanceMaster";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@GodownCode", openingBalanceList.GodownCode);
                    sqlCommand.Parameters.AddWithValue("@RegionCode", openingBalanceList.RegionCode);
                    sqlCommand.Parameters.AddWithValue("@CommodityCode", openingBalanceList.CommodityCode);
                    sqlCommand.Parameters.AddWithValue("@BookBalanceBags", openingBalanceList.BookBalanceBags);
                    sqlCommand.Parameters.AddWithValue("@BookBalanceWeight", openingBalanceList.BookBalanceWeight);
                    sqlCommand.Parameters.AddWithValue("@PhysicalBalanceBags", openingBalanceList.PhysicalBalanceBags);
                    sqlCommand.Parameters.AddWithValue("@PhysicalBalanceWeight", openingBalanceList.PhysicalBalanceWeight);
                    sqlCommand.Parameters.AddWithValue("@CumulitiveShortage", openingBalanceList.CumulitiveShortage);
                    sqlCommand.Parameters.AddWithValue("@ObDate", openingBalanceList.ObDate);
                    sqlCommand.Parameters.AddWithValue("@PurchaseRate", "0");
                    //  sqlCommand.Parameters.AddWithValue("@CurYear", "0");
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@Excess", "0");
                    sqlCommand.Parameters.AddWithValue("@Flag1", "");
                    sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                    sqlCommand.Parameters.AddWithValue("@Receipts", "0.0");
                    sqlCommand.Parameters.AddWithValue("@Issues", "0.0");
                    sqlCommand.Parameters.AddWithValue("@TransferIn", "0.0");
                    sqlCommand.Parameters.AddWithValue("@TransferOut", "0.0");
                    sqlCommand.Parameters.AddWithValue("@WriteOff", "0.0");
                    sqlCommand.Parameters.AddWithValue("@ExcessReceipt", "0.0");
                    sqlCommand.Parameters.AddWithValue("@Others", "0.0");
                    sqlCommand.Parameters.AddWithValue("@Balance", "0");
                    //sqlCommand.Parameters.Add("@Rowid", SqlDbType.NVarChar, 26);
                    //sqlCommand.Parameters["@Rowid"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    //  Rowid = (string)sqlCommand.Parameters["@Rowid"].Value;

                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();
                    //   objTrans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.Dispose();
                    ds.Dispose();
                    dataAdapter = null;
                }
            }

        }


        public bool InsertStackOpening(StackOpeningEntity stackOpeningEntity)
        {
            // string Rowid = string.Empty;
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
                    sqlCommand.CommandText = "InsertStackDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@GodownCode", stackOpeningEntity.GodownCode);
                    sqlCommand.Parameters.AddWithValue("@CommodityCode", stackOpeningEntity.CommodityCode);
                    sqlCommand.Parameters.AddWithValue("@StackNo", stackOpeningEntity.StackNo);
                    sqlCommand.Parameters.AddWithValue("@StackBalanceBags", stackOpeningEntity.Bags);
                    sqlCommand.Parameters.AddWithValue("@StackBalanceWeight", stackOpeningEntity.Weights);
                    sqlCommand.Parameters.AddWithValue("@ObStackDate", stackOpeningEntity.ObStackDate);
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@RegionCode", stackOpeningEntity.RegionCode);
                    sqlCommand.Parameters.AddWithValue("@Flag1", "R");
                    sqlCommand.Parameters.AddWithValue("@Flag2", "0");
                    //sqlCommand.Parameters.AddWithValue("@clstackdate", stackOpeningEntity.clstackdate);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();
                    //   objTrans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.Dispose();
                    ds.Dispose();
                    dataAdapter = null;
                }
            }
        }

    }
}
