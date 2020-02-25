using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TNCSCAPI.DataTransfer;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageSQL
{
    public class ManageTruckMemo
    {

        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        SqlDataAdapter dataAdapter;

        /// <summary>
        /// Insert Delivery order details
        /// </summary>
        /// <param name="deliveryOrderEntity">Delivery order details entity</param>
        /// <returns></returns>
        public Tuple<bool, string, string> InsertTruckMemoEntry(DocumentStockTransferDetails documentStockTransferDetails)
        {
            SqlTransaction objTrans = null;
            string RowID = string.Empty, STNo = string.Empty;
            bool isNewDoc = true;
            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
            {
                DataSet ds = new DataSet();

                sqlCommand = new SqlCommand();
                try
                {
                    if (documentStockTransferDetails.STNo.Length > 5)
                    {
                        isNewDoc = false;
                    }
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    objTrans = sqlConnection.BeginTransaction();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertStockTransferDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@STDate", documentStockTransferDetails.STDate);
                    sqlCommand.Parameters.AddWithValue("@TrCode", documentStockTransferDetails.TrCode);
                    sqlCommand.Parameters.AddWithValue("@MNo", documentStockTransferDetails.MNo);
                    sqlCommand.Parameters.AddWithValue("@MDate", documentStockTransferDetails.MDate);
                    sqlCommand.Parameters.AddWithValue("@RNo", documentStockTransferDetails.RNo);
                    sqlCommand.Parameters.AddWithValue("@RDate", documentStockTransferDetails.RDate);
                    sqlCommand.Parameters.AddWithValue("@ReceivingCode", documentStockTransferDetails.ReceivingCode);
                    sqlCommand.Parameters.AddWithValue("@IssuingCode", documentStockTransferDetails.IssuingCode);
                    sqlCommand.Parameters.AddWithValue("@RCode", documentStockTransferDetails.RCode);
                    sqlCommand.Parameters.AddWithValue("@GunnyUtilised", documentStockTransferDetails.GunnyUtilised);
                    sqlCommand.Parameters.AddWithValue("@GunnyReleased", documentStockTransferDetails.GunnyReleased);
                    sqlCommand.Parameters.AddWithValue("@IssueSlip", documentStockTransferDetails.IssueSlip);
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@Flag1", documentStockTransferDetails.ManualDocNo);
                    sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                    sqlCommand.Parameters.AddWithValue("@TruckMemo", "F");
                    sqlCommand.Parameters.AddWithValue("@STNo1", documentStockTransferDetails.STNo);
                    sqlCommand.Parameters.AddWithValue("@RowId1", documentStockTransferDetails.RowId);
                    sqlCommand.Parameters.Add("@STNo", SqlDbType.NVarChar, 10);
                    sqlCommand.Parameters.Add("@RowId", SqlDbType.NVarChar, 30);
                    sqlCommand.Parameters["@STNo"].Direction = ParameterDirection.Output;
                    sqlCommand.Parameters["@RowId"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    RowID = (string)sqlCommand.Parameters["@RowId"].Value;
                    STNo = (string)sqlCommand.Parameters["@STNo"].Value;

                    documentStockTransferDetails.STNo = STNo;
                    ManageDocumentTruckMemo documentTruckMemo = new ManageDocumentTruckMemo();
                    Task.Run(() => documentTruckMemo.GenerateTruckMemo(documentStockTransferDetails));

                    if (isNewDoc)
                    {
                        if (CheckValidGodown(documentStockTransferDetails.ReceivingCode.Trim()))
                        {
                            ManageDataTransfer dataTransfer = new ManageDataTransfer();
                            DataTransferEntity transferEntity = new DataTransferEntity();
                            transferEntity.DocNumber = STNo;
                            transferEntity.DocDate = documentStockTransferDetails.STDate;
                            transferEntity.DocType = 3;
                            transferEntity.TripType = 1;
                            transferEntity.RCode = documentStockTransferDetails.RCode;
                            transferEntity.GCode = documentStockTransferDetails.IssuingCode;
                            transferEntity.G2GStatus = 4;
                            transferEntity.GPSStatus = 0;
                            dataTransfer.InsertDataTransfer(transferEntity);
                        }
                    }

                    //Delete Sr Item Details
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "DeleteStockTransferDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@STNo", STNo);
                    sqlCommand.ExecuteNonQuery();

                    //Insert data into documentSTItemDetails
                    foreach (var item in documentStockTransferDetails.documentSTItemDetails)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertSTItemDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@STNo", STNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@TStockNo", item.TStockNo);
                        sqlCommand.Parameters.AddWithValue("@ICode", item.ICode);
                        sqlCommand.Parameters.AddWithValue("@IPCode", item.IPCode);
                        sqlCommand.Parameters.AddWithValue("@NoPacking", item.NoPacking);
                        sqlCommand.Parameters.AddWithValue("@WTCode", item.WTCode);
                        sqlCommand.Parameters.AddWithValue("@GKgs", item.GKgs);
                        sqlCommand.Parameters.AddWithValue("@Nkgs", item.Nkgs);
                        sqlCommand.Parameters.AddWithValue("@Moisture", item.Moisture);
                        sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
                        sqlCommand.Parameters.AddWithValue("@Rcode", item.RCode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("@Flag1", item.StackYear);
                        sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }

                    //Insert data into documentSTTDetails
                    foreach (var item in documentStockTransferDetails.documentSTTDetails)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();
                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertSTTDetails";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@STNo", STNo);
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@TransportMode", item.TransportMode);
                        sqlCommand.Parameters.AddWithValue("@TransporterName", item.TransporterName);
                        sqlCommand.Parameters.AddWithValue("@LWBillNo", item.LWBillNo);
                        sqlCommand.Parameters.AddWithValue("@LWBillDate", item.LWBillDate);
                        sqlCommand.Parameters.AddWithValue("@FreightAmount", item.FreightAmount);
                        sqlCommand.Parameters.AddWithValue("@Kilometers", item.Kilometers);
                        sqlCommand.Parameters.AddWithValue("@WHDNo", item.WHDNo);
                        sqlCommand.Parameters.AddWithValue("@WCharges", item.WCharges);
                        sqlCommand.Parameters.AddWithValue("@HCharges", item.HCharges);
                        sqlCommand.Parameters.AddWithValue("@FStation", item.FStation);
                        sqlCommand.Parameters.AddWithValue("@TStation", item.TStation);
                        sqlCommand.Parameters.AddWithValue("@Remarks", item.Remarks);
                        sqlCommand.Parameters.AddWithValue("@FCode", item.FCode);
                        sqlCommand.Parameters.AddWithValue("@Vcode", item.Vcode);
                        sqlCommand.Parameters.AddWithValue("@LDate", item.LDate);
                        sqlCommand.Parameters.AddWithValue("@LNo", item.LNo);
                        sqlCommand.Parameters.AddWithValue("@Wno", item.Wno);
                        sqlCommand.Parameters.AddWithValue("@RRNo", item.RRNo);
                        sqlCommand.Parameters.AddWithValue("@RailHead", item.RailHead);
                        sqlCommand.Parameters.AddWithValue("@RFreightAmount", item.RFreightAmount);
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
                    return new Tuple<bool, string, string>(true, GlobalVariable.SavedMessage + STNo, STNo);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string, string>(false, GlobalVariable.ErrorMessage, "");
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
        public bool CheckValidGodown(string GCode)
        {
            bool isActive = false;
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                sqlParameters1.Add(new KeyValuePair<string, string>("@TNCSCode", GCode));
                var result1 = manageSQLConnection.GetDataSetValues("GetValidGodownForGPS", sqlParameters1);
                if (result1 != null)
                {
                    if (result1.Tables[0].Rows.Count > 0)
                    {
                        isActive = true;
                    }
                }

            }
            catch (Exception ex)
            {
                AuditLog.WriteError("CheckValidGodown : " + ex.Message + " " + ex.StackTrace);
                isActive = false;
            }
            return isActive;
        }
    }
}
