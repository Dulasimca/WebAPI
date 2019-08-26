using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageSQL
{
    public class ManageDeliveryOrder
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
         /// <summary>
        /// Insert Delivery order details
        /// </summary>
        /// <param name="deliveryOrderEntity">Delivery order details entity</param>
        /// <returns></returns>
        public Tuple<bool,string> InsertDeliveryOrderEntry(DocumentDeliveryOrderEntity deliveryOrderEntity)
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
                    sqlCommand.CommandText = "InsertDeliveryOrderDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@DoDate", deliveryOrderEntity.DoDate);
                    sqlCommand.Parameters.AddWithValue("@TransactionCode", deliveryOrderEntity.TransactionCode);
                    sqlCommand.Parameters.AddWithValue("@IndentNo", deliveryOrderEntity.IndentNo);
                    sqlCommand.Parameters.AddWithValue("@PermitDate", deliveryOrderEntity.PermitDate);
                    sqlCommand.Parameters.AddWithValue("@OrderPeriod", deliveryOrderEntity.OrderPeriod);
                    sqlCommand.Parameters.AddWithValue("@ReceivorCode", deliveryOrderEntity.ReceivorCode);
                    sqlCommand.Parameters.AddWithValue("@IssuerCode", deliveryOrderEntity.IssuerCode);
                    sqlCommand.Parameters.AddWithValue("@IssuerType", deliveryOrderEntity.IssuerType);
                    sqlCommand.Parameters.AddWithValue("@GrandTotal", deliveryOrderEntity.GrandTotal);
                    sqlCommand.Parameters.AddWithValue("@Regioncode", deliveryOrderEntity.Regioncode);
                    sqlCommand.Parameters.AddWithValue("@Remarks", deliveryOrderEntity.Remarks);
                    sqlCommand.Parameters.AddWithValue("@deliverytype", deliveryOrderEntity.deliverytype);
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@Flag1", "-");
                    sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                    sqlCommand.Parameters.AddWithValue("@Dono1", deliveryOrderEntity.Dono);
                    sqlCommand.Parameters.AddWithValue("@RowId1", deliveryOrderEntity.RowId);
                    sqlCommand.Parameters.Add("@Dono", SqlDbType.NVarChar, 10);
                    sqlCommand.Parameters.Add("@RowId", SqlDbType.NVarChar, 30);
                    sqlCommand.Parameters["@Dono"].Direction = ParameterDirection.Output;
                    sqlCommand.Parameters["@RowId"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    RowID = (string)sqlCommand.Parameters["@RowId"].Value;
                    SRNo = (string)sqlCommand.Parameters["@Dono"].Value;
                    deliveryOrderEntity.Dono = SRNo;
                    deliveryOrderEntity.RowId = RowID;

                    //#if (!DEBUG)
                        ManageDocumentDeliveryOrder documentDO = new ManageDocumentDeliveryOrder();
                        Task.Run(() => documentDO.GenerateDeliveryOrderText(deliveryOrderEntity));
                    //#else
                    //    ManageDocumentDeliveryOrder documentDO = new ManageDocumentDeliveryOrder();
                    //    documentDO.GenerateDeliveryOrderText(deliveryOrderEntity);
                    //#endif


                    //Delete Sr Item Details
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "DeleteDeliveryOrderDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Dono", SRNo);
                    sqlCommand.ExecuteNonQuery();

                    //Insert data into SR Item details
                    foreach (var item in deliveryOrderEntity.documentDeliveryItems)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertDeliveryItemDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Dono", SRNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@Itemcode", item.Itemcode);
                        sqlCommand.Parameters.AddWithValue("@NetWeight", item.NetWeight);
                        sqlCommand.Parameters.AddWithValue("@Wtype", item.Wtype);
                        sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
                        sqlCommand.Parameters.AddWithValue("@Rate", item.Rate);
                        sqlCommand.Parameters.AddWithValue("@Total", item.Total);
                        sqlCommand.Parameters.AddWithValue("@Rcode", item.Rcode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("Flag1", "-");
                        sqlCommand.Parameters.AddWithValue("Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }

                    //Insert data into SR Item details
                    foreach (var item in deliveryOrderEntity.deliveryPaymentDetails)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertDeliveryPaymentDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Dono", SRNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@PaymentMode", item.PaymentMode);
                        sqlCommand.Parameters.AddWithValue("@PaymentAmount", item.PaymentAmount);
                        sqlCommand.Parameters.AddWithValue("@ChequeNo", item.ChequeNo);
                        sqlCommand.Parameters.AddWithValue("@ChDate", item.ChDate);
                        sqlCommand.Parameters.AddWithValue("@bank", item.bank);
                        sqlCommand.Parameters.AddWithValue("@payableat", item.payableat);
                        sqlCommand.Parameters.AddWithValue("@Rcode", item.Rcode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("Flag1", "-");
                        sqlCommand.Parameters.AddWithValue("Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }

                    //Insert data into SR Item details
                    foreach (var item in deliveryOrderEntity.deliveryMarginDetails)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertDeliveryMarginDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Dono", SRNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@SchemeCode", item.SchemeCode);
                        sqlCommand.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                        sqlCommand.Parameters.AddWithValue("@MarginWtype", item.MarginWtype);
                        sqlCommand.Parameters.AddWithValue("@MarginNkgs", item.MarginNkgs);
                        sqlCommand.Parameters.AddWithValue("@MarginRate", item.MarginRate);
                        sqlCommand.Parameters.AddWithValue("@MarginAmount", item.MarginAmount);
                        sqlCommand.Parameters.AddWithValue("@Rcode", item.Rcode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("Flag1", "-");
                        sqlCommand.Parameters.AddWithValue("Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }


                    ////Insert data into SR Item details
                    foreach (var item in deliveryOrderEntity.deliveryAdjustmentDetails)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertDeliveryAdjustmentDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Dono", SRNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@AdjustedDoNo", item.AdjustedDoNo);
                        sqlCommand.Parameters.AddWithValue("@Amount", item.Amount);
                        sqlCommand.Parameters.AddWithValue("@AdjustmentType", item.AdjustmentType);
                        sqlCommand.Parameters.AddWithValue("@AdjustDate", item.AdjustDate);
                        sqlCommand.Parameters.AddWithValue("@AmountNowAdjusted", item.AmountNowAdjusted);
                        sqlCommand.Parameters.AddWithValue("@Balance", item.Balance);
                        sqlCommand.Parameters.AddWithValue("@Rcode", item.Rcode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("Flag1", "-");
                        sqlCommand.Parameters.AddWithValue("Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }

                    //Insert SRT Details table
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();
                    objTrans.Commit();
                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage + SRNo);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string> (false, GlobalVariable.ErrorMessage);
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
