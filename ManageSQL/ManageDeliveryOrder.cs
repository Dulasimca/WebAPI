using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageSQL
{
    public class ManageDeliveryOrder
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        SqlTransaction objTrans = null;
        /// <summary>
        /// Insert Delivery order details
        /// </summary>
        /// <param name="deliveryOrderEntity">Delivery order details entity</param>
        /// <returns></returns>
        public Tuple<bool, string> InsertDeliveryOrderEntry(DocumentDeliveryOrderEntity deliveryOrderEntity)
        {

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

                    //Insert Data into Sales tax detail table
                    InsertSalesTaxDetails(deliveryOrderEntity, SRNo);

                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();
                    objTrans.Commit();
                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage + SRNo);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
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

        public void InsertSalesTaxDetails(DocumentDeliveryOrderEntity deliveryOrderEntity, string SRNo)
        {
            if (deliveryOrderEntity.DOTaxStatus == "YES")
            {
                //Get values to calculate the GST value.
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@EffectDate", deliveryOrderEntity.DoDate));
                ds = manageSQLConnection.GetDataSetValues("GetRateMasterData", sqlParameters);
                ManageReport report = new ManageReport();
                if (report.CheckDataAvailable(ds))
                {
                    List<RateEntity> _rateEntity = new List<RateEntity>();
                    _rateEntity = report.ConvertDataTableToList<RateEntity>(ds.Tables[0]);

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "DeleteDOSalesTaxDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BillNo", SRNo);
                    sqlCommand.Parameters.AddWithValue("@GCode", deliveryOrderEntity.IssuerCode);
                    sqlCommand.ExecuteNonQuery();

                    var creditSales = (deliveryOrderEntity.TransactionCode == "TR019") ? true : false;
                    decimal gst = 0, taxPercent = 0, taxAmnt = 0, rate = 0, amnt = 0;
                    foreach (var item in deliveryOrderEntity.documentDeliveryItems)
                    {
                        //Filter the value based on HSN no and Scheme.
                        var result = (from a in _rateEntity
                                      where a.Hsncode == item.HsnCode && a.Scheme == item.Scheme
                                      select a).FirstOrDefault();
                    
                        if (result != null)
                        {
                            item.TaxPercent = Convert.ToString(result.TaxPercentage);
                            rate = result.Rate;
                        }

                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertIntoGSTSalesTax";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@BillNo", SRNo);
                        sqlCommand.Parameters.AddWithValue("@BillDate", deliveryOrderEntity.DoDate);
                        sqlCommand.Parameters.AddWithValue("@Month", deliveryOrderEntity.Month);
                        sqlCommand.Parameters.AddWithValue("@Year", deliveryOrderEntity.Year);
                        sqlCommand.Parameters.AddWithValue("@CompanyID", deliveryOrderEntity.PartyID);
                        sqlCommand.Parameters.AddWithValue("@CreditSales", creditSales);
                        sqlCommand.Parameters.AddWithValue("@TaxPercentage", item.TaxPercent);
                        sqlCommand.Parameters.AddWithValue("@Hsncode", item.HsnCode);
                        sqlCommand.Parameters.AddWithValue("@CommodityID", item.Itemcode);
                        sqlCommand.Parameters.AddWithValue("@TaxType", "CGST");
                        sqlCommand.Parameters.AddWithValue("@Measurement", item.Wtype);
                        sqlCommand.Parameters.AddWithValue("@Quantity", item.NetWeight);
                        taxPercent = (Convert.ToDecimal(item.TaxPercent) / 2);
                     //   rate = ((Convert.ToDouble(item.Rate)) - ((Convert.ToDouble(item.Rate) * Convert.ToDouble(item.TaxPercent)) / 100));
                        sqlCommand.Parameters.AddWithValue("@Rate", rate);
                        amnt = (Convert.ToDecimal(item.NetWeight) * rate);
                        sqlCommand.Parameters.AddWithValue("@Amount", amnt);
                        gst = ((amnt * taxPercent) / 100);
                        sqlCommand.Parameters.AddWithValue("@CGST", gst);
                        sqlCommand.Parameters.AddWithValue("@SGST", gst);
                        taxAmnt = (gst * 2);
                        sqlCommand.Parameters.AddWithValue("@TaxAmount", taxAmnt);
                        sqlCommand.Parameters.AddWithValue("@Total", (amnt + taxAmnt));
                        sqlCommand.Parameters.AddWithValue("@RCode", item.Rcode);
                        sqlCommand.Parameters.AddWithValue("@GCode", deliveryOrderEntity.IssuerCode);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", deliveryOrderEntity.UserID);
                        sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
                        sqlCommand.Parameters.AddWithValue("@DORate", item.Rate);
                        sqlCommand.Parameters.AddWithValue("@DOTotal", item.Total);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

    }
    public class RateEntity
    {
        public decimal Rate { get; set; }
        public string Scheme { get; set; }
        public string Hsncode { get; set; }
        public decimal TaxPercentage { get; set; }
    }
}
