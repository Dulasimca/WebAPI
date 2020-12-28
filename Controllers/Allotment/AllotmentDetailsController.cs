using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNCSCAPI.Models.Allotment;

namespace TNCSCAPI.Controllers.Allotment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllotmentDetailsController : ControllerBase
    {
        public Tuple<bool, string> Post(List<AllotmentDataEntity> entity)
        {
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
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
                foreach (var data in entity)
                {
                    foreach (var item in data.ShopItemList)
                    {
                        foreach (var i in item.ItemList)
                        {

                            dt.Rows.Add(0, "", item.FPSCode, "-", i.ITCode, Convert.ToDouble(i.Quantity), data.AllotmentMonth
                                , data.AllotmentYear, data.GCode, data.Taluk);
                        }
                    }
                }
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
                        sqlCommand.Parameters.AddWithValue("@TableAllotmentQuantity", dt);
                        sqlCommand.ExecuteNonQuery();
                        objTrans.Commit();
                        return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
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