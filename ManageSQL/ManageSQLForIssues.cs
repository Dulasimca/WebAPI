using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TNCSCAPI.DataTransfer;
using TNCSCAPI.ManageAllReports.Document;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI
{
    public class ManageSQLForIssues
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string,string> InsertIssuesEntry(DocumentStockIssuesEntity issueList)
        {
            SqlTransaction objTrans = null;
            string RowID = string.Empty, SINo = string.Empty;
          //  bool isNewDoc = true;
            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
            {
                DataSet ds = new DataSet();

                sqlCommand = new SqlCommand();
                try
                {
                    //if (issueList.SINo.Length > 5)
                    //{
                    //    isNewDoc = false;
                    //}
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    objTrans = sqlConnection.BeginTransaction();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "InsertStockIssueDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SIDate", issueList.SIDate);
                    sqlCommand.Parameters.AddWithValue("@Trcode", issueList.Trcode);
                    sqlCommand.Parameters.AddWithValue("@IRelates", issueList.IRelates);
                    sqlCommand.Parameters.AddWithValue("@DNo", issueList.DNo);
                    sqlCommand.Parameters.AddWithValue("@DDate", issueList.DDate);
                    sqlCommand.Parameters.AddWithValue("@WCCode", issueList.WCCode);
                    sqlCommand.Parameters.AddWithValue("@IssuingCode", issueList.IssuingCode);
                    sqlCommand.Parameters.AddWithValue("@Receivorcode", issueList.Receivorcode);
                    sqlCommand.Parameters.AddWithValue("@issuetype1", issueList.Issuetype);
                    sqlCommand.Parameters.AddWithValue("@SoundServiceable", issueList.SoundServiceable);
                    sqlCommand.Parameters.AddWithValue("@ServiceablePatches", issueList.ServiceablePatches);
                    sqlCommand.Parameters.AddWithValue("@Gunnyutilised", issueList.GunnyUtilised);
                    sqlCommand.Parameters.AddWithValue("@GunnyReleased", issueList.GunnyReleased);
                    sqlCommand.Parameters.AddWithValue("@Remarks", issueList.Remarks);
                    sqlCommand.Parameters.AddWithValue("@TransporterName", issueList.TransporterName);
                    sqlCommand.Parameters.AddWithValue("@TransportingCharge", issueList.TransportingCharge);
                    sqlCommand.Parameters.AddWithValue("@LorryNo", issueList.LorryNo);
                    sqlCommand.Parameters.AddWithValue("@NewBale", issueList.NewBale);
                    sqlCommand.Parameters.AddWithValue("@RCode", issueList.RCode);
                    sqlCommand.Parameters.AddWithValue("@IssueType", "-");
                    sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                    sqlCommand.Parameters.AddWithValue("@Loadingslip", issueList.Loadingslip);
                    sqlCommand.Parameters.AddWithValue("@IssueMemo", "-");
                    sqlCommand.Parameters.AddWithValue("@Flag1", issueList.ManualDocNo);
                    sqlCommand.Parameters.AddWithValue("@Flag2", issueList.IssueRegularAdvance);
                    sqlCommand.Parameters.AddWithValue("@SINo1", issueList.SINo != null ? issueList.SINo : "");
                    sqlCommand.Parameters.AddWithValue("@RowId1", issueList.RowId != null ? issueList.RowId : "");
                    sqlCommand.Parameters.Add("@SINo", SqlDbType.NVarChar, 13);
                    sqlCommand.Parameters.Add("@RowId", SqlDbType.NVarChar, 30);
                    sqlCommand.Parameters["@SINo"].Direction = ParameterDirection.Output;
                    sqlCommand.Parameters["@RowId"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    RowID = Convert.ToString(sqlCommand.Parameters["@RowId"].Value);
                    SINo = Convert.ToString(sqlCommand.Parameters["@SINo"].Value);
                    issueList.SINo = SINo;

                    //#if (!DEBUG)
                    ManageDocumentIssues documentIssues = new ManageDocumentIssues();
                    Task.Run(() => documentIssues.GenerateIssues(issueList));

                    //if (isNewDoc)
                    //{
                    //    ManageDataTransfer dataTransfer = new ManageDataTransfer();
                    //    DataTransferEntity transferEntity = new DataTransferEntity();
                    //    transferEntity.DocNumber = SINo;
                    //    transferEntity.DocType = 2;
                    //    transferEntity.TripType = 2;
                    //    transferEntity.RCode = issueList.RCode;
                    //    transferEntity.GCode = issueList.IssuingCode;
                    //    // dataTransfer.InsertDataTransfer(transferEntity);
                    //    Task.Run(() => dataTransfer.InsertDataTransfer(transferEntity));

                    //}


                    //   Delete Stock issue Item Details
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = objTrans;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "DeleteSIItemDetails";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SINo", SINo);
                    sqlCommand.ExecuteNonQuery();

                    //Insert data into SI Item details

                    List<DocumentStockIssuesItemEntity> stockIssuesItemEntities = new List<DocumentStockIssuesItemEntity>();
                    stockIssuesItemEntities = issueList.IssueItemList;
                    foreach (var item in stockIssuesItemEntities)
                    {
                        if (item.TStockNo.ToUpper() != "TOTAL")
                        {
                            sqlCommand.Parameters.Clear();
                            sqlCommand.Dispose();

                            sqlCommand = new SqlCommand();
                            sqlCommand.Transaction = objTrans;
                            sqlCommand.Connection = sqlConnection;
                            sqlCommand.CommandText = "InsertSIItemDetails";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                            sqlCommand.Parameters.AddWithValue("@SINo", SINo);
                            sqlCommand.Parameters.AddWithValue("@TStockNo", item.TStockNo);
                            sqlCommand.Parameters.AddWithValue("@ICode", item.ICode);
                            sqlCommand.Parameters.AddWithValue("@IPCode", item.IPCode);
                            sqlCommand.Parameters.AddWithValue("@NoPacking", item.NoPacking);
                            sqlCommand.Parameters.AddWithValue("@WTCode", item.WTCode);
                            sqlCommand.Parameters.AddWithValue("@GKgs", item.GKgs);
                            sqlCommand.Parameters.AddWithValue("@Nkgs", item.Nkgs);
                            sqlCommand.Parameters.AddWithValue("@Moisture", item.Moisture);
                            sqlCommand.Parameters.AddWithValue("@Scheme", item.Scheme);
                            sqlCommand.Parameters.AddWithValue("@RCode", issueList.RCode);
                            sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                            sqlCommand.Parameters.AddWithValue("@flag1", item.StackYear); //StackYear
                            sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                            sqlCommand.ExecuteNonQuery();
                        }
                    }

                    //Insert data into IssueMemoDoNo Item details

                    List<DocumentStockIssueDetailsEntity> stockIssueDetailsEntities = new List<DocumentStockIssueDetailsEntity>();
                    stockIssueDetailsEntities = issueList.SIDetailsList;
                    foreach (var item in stockIssueDetailsEntities)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "InsertIssuememodono";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Issuememono", SINo);
                        sqlCommand.Parameters.AddWithValue("@IssueMemoDate", item.SIDate);
                        sqlCommand.Parameters.AddWithValue("@DeliveryOrderNo", item.DNo);
                        sqlCommand.Parameters.AddWithValue("@DeliveryOrderDate", item.DDate);
                        sqlCommand.Parameters.AddWithValue("@Godowncode", item.GodownCode);
                        sqlCommand.Parameters.AddWithValue("@RCode", issueList.RCode);
                        sqlCommand.Parameters.AddWithValue("@ExportFlag", "N");
                        sqlCommand.Parameters.AddWithValue("@RowId", RowID);
                        sqlCommand.Parameters.AddWithValue("@Flag1", "-");
                        sqlCommand.Parameters.AddWithValue("@Flag2", "-");
                        sqlCommand.ExecuteNonQuery();
                    }
                    System.Threading.Thread.Sleep(100);
                    objTrans.Commit();
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Dispose();

                    return new Tuple<bool, string,string>(true, GlobalVariable.SavedMessage + SINo, SINo);

                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                   objTrans.Rollback();
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
