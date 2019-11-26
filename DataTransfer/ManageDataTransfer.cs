using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.DataTransfer
{
    public class ManageDataTransfer
    {
        /// <summary>
        /// Insert details to data transfer's table.
        /// </summary>
        /// <param name="dataTransferEntity">Data Transfer entity</param>
        public void InsertDataTransfer(DataTransferEntity dataTransferEntity)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                DataSet nds = new DataSet();
                ManageSQLConnection manageSQL = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@TNCSCKey", "GPS"));
                nds = manageSQL.GetDataSetValues("GetTNCSCSettings", sqlParameters);
                if (nds.Tables.Count > 0)
                {
                    if (nds.Tables[0].Rows.Count > 0)
                    {
                        string sValue = nds.Tables[0].Rows[0]["TNCSCValue"].ToString();
                        sValue = !string.IsNullOrEmpty(sValue) ? sValue.ToUpper() : sValue;
                        if (sValue == "YES")
                        {
                            using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
                            {
                                if (sqlConnection.State == 0)
                                {
                                    sqlConnection.Open();
                                }
                                sqlCommand.Parameters.Clear();
                                sqlCommand.Dispose();
                                sqlCommand = new SqlCommand();
                                sqlCommand.Connection = sqlConnection;
                                sqlCommand.CommandText = "InsertDataTransferDetails";
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.AddWithValue("@GodownCode", dataTransferEntity.GCode);
                                sqlCommand.Parameters.AddWithValue("@RegionCode", dataTransferEntity.RCode);
                                sqlCommand.Parameters.AddWithValue("@DocType", dataTransferEntity.DocType);//"3"
                                sqlCommand.Parameters.AddWithValue("@TripType", dataTransferEntity.TripType);//1
                                sqlCommand.Parameters.AddWithValue("@DocNumber", dataTransferEntity.DocNumber);
                                sqlCommand.Parameters.AddWithValue("@GPSStatus", dataTransferEntity.GPSStatus);
                                sqlCommand.Parameters.AddWithValue("@GToGStatus", dataTransferEntity.G2GStatus);
                                sqlCommand.Parameters.Add("@TransferId", SqlDbType.BigInt);
                                sqlCommand.Parameters["@TransferId"].Direction = ParameterDirection.Output;
                                sqlCommand.ExecuteNonQuery();

                                //Int64 Transferid = (Int64)sqlCommand.Parameters["@TransferId"].Value;
                                //string[] split = dataTransferEntity.DocNumber.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                //foreach (string item in split)
                                //{
                                //    sqlCommand.Parameters.Clear();
                                //    sqlCommand.Dispose();
                                //    sqlCommand = new SqlCommand();
                                //    sqlCommand.Connection = sqlConnection;
                                //    sqlCommand.CommandText = "InsertDataTransferItemDetails";
                                //    sqlCommand.CommandType = CommandType.StoredProcedure;
                                //    sqlCommand.Parameters.AddWithValue("@TransferId", Transferid.ToString());
                                //    sqlCommand.Parameters.AddWithValue("@DocNumber", item);
                                //    sqlCommand.ExecuteNonQuery();
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("InsertDataTransfer : " + ex.Message + " " + ex.InnerException);
            }
        }
    }
}
