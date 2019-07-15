using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Models
{
    public class ManageStock
    {
        /// <summary>
        /// Get the daily stock statement .
        /// </summary>
        /// <param name="dataSet">dataset for all table. item, region and godown wise data</param>
        /// <returns></returns>
        public List<DailyStockStatment> GetDailyStockStatments(DataSet dataSet)
        {
            try
            {
                List<DailyStockStatment> dailyStockStatments = new List<DailyStockStatment>();
                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        DailyStockStatment dailyStock = new DailyStockStatment
                        {
                            ItemCode = Convert.ToString(dr["ItemCode"]),
                            Name = Convert.ToString(dr["ITDescription"]),
                            OpeningBalance = Convert.ToString(dr["OpeningBalance"]),
                            ClosingBalance = Convert.ToString(dr["ClosingBalance"]),
                            PhycialBalance = Convert.ToString(dr["PhycialBalance"]),
                            CSBalance = Convert.ToString(dr["CSBalance"]),
                            Shortage = Convert.ToString(dr["Shortage"]),
                            TotalReceipt = Convert.ToString(dr["TotalReceipt"]),
                            IssueSales = Convert.ToString(dr["IssueSales"]),
                            IssueOthers = Convert.ToString(dr["IssueOthers"]),
                            LastUpdated = Convert.ToDateTime(dr["LastUpdated"]),
                            ListItems = GetRegionwiseStock(dataSet, Convert.ToString(dr["ItemCode"]))
                        };
                        dailyStockStatments.Add(dailyStock);
                    }
                }
                return dailyStockStatments;
            }
            catch (Exception ex)
            {
                throw;
            }
         
        }
        /// <summary>
        /// Get the region wise daily statement for a items.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="ItemCode">item code</param>
        /// <returns></returns>
        public List<DailyStockStatment> GetRegionwiseStock(DataSet dataSet, string ItemCode)
        {
            try
            {
                List<DailyStockStatment> dailyStockStatments = new List<DailyStockStatment>();
                if (dataSet.Tables.Count > 1)
                {
                    DataRow[] dataRows = dataSet.Tables[1].Select("ItemCode='"+ ItemCode + "'");
                    foreach (DataRow dr in dataRows)
                    {
                        DailyStockStatment dailyStock = new DailyStockStatment
                        {
                            ItemCode= Convert.ToString(dr["ItemCode"]),
                            Name=Convert.ToString(dr["RGNAME"]),
                            RegionCode= Convert.ToString(dr["RGCODE"]),
                            OpeningBalance =  Convert.ToString(dr["OpeningBalance"]),
                            ClosingBalance = Convert.ToString(dr["ClosingBalance"]),
                            PhycialBalance = Convert.ToString(dr["PhycialBalance"]),
                            CSBalance = Convert.ToString(dr["CSBalance"]),
                            Shortage = Convert.ToString(dr["Shortage"]),
                            TotalReceipt = Convert.ToString(dr["TotalReceipt"]),
                            IssueSales = Convert.ToString(dr["IssueSales"]),
                            IssueOthers = Convert.ToString(dr["IssueOthers"]),
                            LastUpdated= Convert.ToDateTime(dr["LastUpdated"]),
                            ListItems= GetGodownwiseStock(dataSet, Convert.ToString(dr["RGCODE"]), ItemCode)
                        };
                        dailyStockStatments.Add(dailyStock);
                    }
                }
                return dailyStockStatments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the godown wise daily statement for a items.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="RegionCode">region code</param>
        /// <returns></returns>
        public List<DailyStockStatment> GetGodownwiseStock(DataSet dataSet, string RegionCode, string ItemCode)
        {
            try
            {
                List<DailyStockStatment> dailyStockStatments = new List<DailyStockStatment>();
                if (dataSet.Tables.Count > 2)
                {
                    DataRow[] dataRows = dataSet.Tables[2].Select("RGCODE='" + RegionCode + "' AND ItemCode='" + ItemCode + "'");
                    foreach (DataRow dr in dataRows)
                    {
                        DailyStockStatment dailyStock = new DailyStockStatment
                        {
                            ItemCode = Convert.ToString(dr["ItemCode"]),
                            Name = Convert.ToString(dr["TNCSName"]),
                            RegionCode = Convert.ToString(dr["RGCODE"]),
                            OpeningBalance = Convert.ToString(dr["OpeningBalance"]),
                            ClosingBalance = Convert.ToString(dr["ClosingBalance"]),
                            PhycialBalance = Convert.ToString(dr["PhycialBalance"]),
                            CSBalance = Convert.ToString(dr["CSBalance"]),
                            Shortage = Convert.ToString(dr["Shortage"]),
                            TotalReceipt = Convert.ToString(dr["TotalReceipt"]),
                            IssueSales = Convert.ToString(dr["IssueSales"]),
                            IssueOthers = Convert.ToString(dr["IssueOthers"]),
                            LastUpdated = Convert.ToDateTime(dr["LastUpdated"])
                        };
                        dailyStockStatments.Add(dailyStock);
                    }
                }
                return dailyStockStatments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public PhycialBalance GetPhycialBalance(DataSet dataSet)
        {
            try
            {
                if (dataSet.Tables.Count > 4)
                {
                    PhycialBalance phycialBalance = new PhycialBalance
                    {
                        RawRice = Convert.ToString(dataSet.Tables[0].Rows[0][0]),
                        BoiledRice = Convert.ToString(dataSet.Tables[1].Rows[0][0]),
                        Dhall = Convert.ToString(dataSet.Tables[2].Rows[0][0]),
                        POil = Convert.ToString(dataSet.Tables[3].Rows[0][0]),
                        Wheat = Convert.ToString(dataSet.Tables[4].Rows[0][0]),
                        Sugar =Convert.ToString(dataSet.Tables[5].Rows[0][0])
                    };
                    return phycialBalance;
                }
                return null;
            }
            catch( Exception ex)
            {
                throw ex;
            }
          
        }
    }

    /// <summary>
    /// Daily stock statement entity
    /// </summary>
    public class DailyStockStatment
    {
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public string GodownCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public string OpeningBalance { get; set; }
        public string ClosingBalance { get; set; }
        public string PhycialBalance { get; set; }
        public string CSBalance { get; set; }
        public string Shortage { get; set; }
        public string TotalReceipt { get; set; }
        public string IssueSales { get; set; }
        public string IssueOthers { get; set; }
        public List<DailyStockStatment> ListItems { get; set; }

    }

    public class PhycialBalance
    {
        public string RawRice { get; set; }
        public string BoiledRice { get; set; }
        public string Dhall { get; set; }        
        public string POil { get; set; }
        public string Wheat { get; set; }
        public string Sugar { get; set; }
    }
}
