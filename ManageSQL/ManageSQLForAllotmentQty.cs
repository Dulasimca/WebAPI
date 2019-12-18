using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TNCSCAPI.Controllers.Allotment;

namespace TNCSCAPI.ManageSQL
{
    public class ManageSQLForAllotmentQty
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertAllotmentQtyEntry(List<AllotmentQuantityEntity> entity)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("RowId", typeof(long)),
                new DataColumn("SocietyCode", typeof(string)),
                new DataColumn("Societyshopcode",typeof(string)),
            new DataColumn("Schemecode",typeof(string)),
            new DataColumn("commoditycode",typeof(string)),
            new DataColumn("Quantity",typeof(float)),
            new DataColumn("Allotmentmonth",typeof(string)),
            new DataColumn("Allotmentyear",typeof(string)),
            new DataColumn("GCode",typeof(string)),
            new DataColumn("Taluk",typeof(string))});
            try
            {
                foreach (var item in entity)
                {
                    foreach (var i in item.ItemList)
                    {

                        dt.Rows.Add("", item.FPSCode, "-", Convert.ToDouble(i.Quantity), item.AllotmentMonth
                            , item.AllotmentYear, item.GCode, item.Taluk);
                        //sqlCommand.Parameters.AddWithValue("@FPSCode", item.FPSCode);
                        //sqlCommand.Parameters.AddWithValue("@SchemeCode", '-');
                        //sqlCommand.Parameters.AddWithValue("@AMonth", item.AllotmentMonth);
                        //sqlCommand.Parameters.AddWithValue("@AYear", item.AllotmentYear);
                        //sqlCommand.Parameters.AddWithValue("@GCode", item.GCode);
                        //sqlCommand.Parameters.AddWithValue("@Taluk", item.Taluk);
                        //sqlCommand.Parameters.AddWithValue("@ITCode", i.ITCode);
                        //sqlCommand.Parameters.AddWithValue("@Quantity", i.Quantity);
                    }
                }
                // RowId bigint IDENTITY(1, 1) NOT NULL,
                // SocietyCode nvarchar(9) NULL,
                //Societyshopcode nvarchar(9) NULL,
                //Schemecode nvarchar(5) NULL,
                //commoditycode nvarchar(5) NULL,
                //Quantity float NULL,
                //   Allotmentmonth nvarchar(3) NULL,
                //Allotmentyear nvarchar(4) NULL,
                //GCode varchar(3) NULL,
                //Taluk varchar(150) NULL
                SqlTransaction objTrans = null;
                using (sqlConnection = new SqlConnection(GlobalVariable.ConnectionString))
                {
                    sqlCommand = new SqlCommand();

                    try
                    {
                        if (sqlConnection.State == 0)
                        {
                            sqlConnection.Open();
                        }
                        objTrans = sqlConnection.BeginTransaction();

                        sqlCommand.Parameters.Clear();
                        sqlCommand.Dispose();

                        sqlCommand = new SqlCommand();
                        sqlCommand.Transaction = objTrans;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "BulkInsertAllotmentQuantity";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        //sqlCommand.Parameters.AddWithValue("@SocietyCode", item.SocietyCode);
                        sqlCommand.Parameters.AddWithValue("@TableAllotmentQuantity", dt);
                        sqlCommand.ExecuteNonQuery();
                        objTrans.Commit();
                    }
                    catch (Exception ex)
                    {

                        AuditLog.WriteError("Allotment" + ex.Message + " : " + ex.StackTrace);
                        return new Tuple<bool, string>(false, GlobalVariable.ErrorMessage);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        sqlCommand.Dispose();
                    }

                    //sqlCommand.Transaction = objTrans;
                    //sqlCommand.Connection = sqlConnection;
                    #region Old code
                    //foreach (var item in entity)
                    //{
                    //    foreach (var i in item.ItemList)
                    //    {
                    //        sqlCommand.Parameters.Clear();
                    //        sqlCommand.Dispose();

                    //        sqlCommand = new SqlCommand();
                    //        sqlCommand.Transaction = objTrans;
                    //        sqlCommand.Connection = sqlConnection;
                    //        sqlCommand.CommandText = "InsertAllotmentQuantity";
                    //        sqlCommand.CommandType = CommandType.StoredProcedure;
                    //        //sqlCommand.Parameters.AddWithValue("@SocietyCode", item.SocietyCode);
                    //        sqlCommand.Parameters.AddWithValue("@FPSCode", item.FPSCode);
                    //        sqlCommand.Parameters.AddWithValue("@SchemeCode", '-');
                    //        sqlCommand.Parameters.AddWithValue("@AMonth", item.AllotmentMonth);
                    //        sqlCommand.Parameters.AddWithValue("@AYear", item.AllotmentYear);
                    //        sqlCommand.Parameters.AddWithValue("@GCode", item.GCode);
                    //        sqlCommand.Parameters.AddWithValue("@Taluk", item.Taluk);
                    //        sqlCommand.Parameters.AddWithValue("@ITCode", i.ITCode);
                    //        sqlCommand.Parameters.AddWithValue("@Quantity", i.Quantity);

                    //        sqlCommand.ExecuteNonQuery();
                    //    }
                    //}
                    #endregion

                    return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
                }

            }
            catch (Exception ex)
            {               
                AuditLog.WriteError("Allotment" + ex.Message + " : " + ex.StackTrace);
                return new Tuple<bool, string>(false, GlobalVariable.ErrorMessage);
            }
          
        }
    }
}
