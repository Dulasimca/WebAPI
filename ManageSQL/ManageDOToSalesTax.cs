using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Controllers.Allotment;
using TNCSCAPI.Models.DoToSales;

namespace TNCSCAPI.ManageSQL
{
    public class ManageDOToSalesTax
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        public Tuple<bool, string> InsertDoToSalesTax(List<DOSalesTaxEntity> entity)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[33] { new DataColumn("GSTSalesID", typeof(int)),
                new DataColumn("Month", typeof(int)),
                new DataColumn("Year",typeof(int)),
                new DataColumn("CompanyID",typeof(string)),//PartyId
                new DataColumn("CommodityID",typeof(string)), //Itemcode
                new DataColumn("TIN",typeof(string)), //GSTNumber
                new DataColumn("BillNo",typeof(string)),//Dono
                new DataColumn("BillDate",typeof(string)),//DoDate
                new DataColumn("CreditSales",typeof(string)),
                new DataColumn("TaxType", typeof(string)),
                new DataColumn("Measurement",typeof(string)),//Wtype
                new DataColumn("Quantity", typeof(float)),//NetWeight
                new DataColumn("Rate", typeof(float)),
                new DataColumn("Amount", typeof(float)),
                new DataColumn("TaxPercentage",typeof(string)),
                new DataColumn("CGST", typeof(float)),
                new DataColumn("SGST", typeof(float)),
                new DataColumn("IGST", typeof(float)),
                new DataColumn("Hsncode", typeof(string)),
                new DataColumn("TaxAmount", typeof(float)),//GSTTOTAL
                new DataColumn("Total", typeof(float)), //TotalAmount
                new DataColumn("RCode", typeof(string)),//Regioncode
                new DataColumn("GCode", typeof(string)),//IssuerCode
                new DataColumn("AccYear", typeof(string)),//Year
                new DataColumn("CreatedBy", typeof(string)),
                new DataColumn("CreatedDate", typeof(string)),//current date
                new DataColumn("Scheme", typeof(string)),
                new DataColumn("isLocked", typeof(bool)),
                new DataColumn("DORate", typeof(float)),//Rate
                new DataColumn("DOTotal", typeof(float)),//DITotal
                new DataColumn("Flag", typeof(bool)),
                new DataColumn("GSTType", typeof(string)),
                new DataColumn("AADS", typeof(string))
  });
            try
            {
                foreach (var item in entity)
                {
                    var creditSales = (item.TransactionCode == "TR019") ? true : false;
                    dt.Rows.Add(0, item.Month, item.Year, item.PartyId, item.Itemcode, item.GSTNumber, 
                        item.Dono, item.DoDate, creditSales, "CGST", item.Wtype, item.NetWeight,
                        item.SalesRate, item.SalesTOTAL, item.TaxPercentage, item.CGST, item.SGST, null, item.Hsncode, 
                        item.GSTTOTAL, item.TotalAmount, item.Regioncode, item.IssuerCode, item.Year,
                        item.CreatedBy, item.CurrentDate, item.Scheme, true, item.Rate, item.DITotal,  true,
                         1, null
                        );
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
                        sqlCommand.CommandText = "BulkInsertDOToSales";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@TableGSTSalesTax", dt);
                        sqlCommand.ExecuteNonQuery();
                        objTrans.Commit();
                        return new Tuple<bool, string>(true, GlobalVariable.SavedMessage);
                    }
                    catch (Exception ex)
                    {

                        AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
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
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                return new Tuple<bool, string>(false, "Error!");
            }

        }

    }
}
