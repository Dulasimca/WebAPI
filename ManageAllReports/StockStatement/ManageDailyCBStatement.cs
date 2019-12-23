using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class ManageDailyCBStatement
    {
        public List<StockCBEntity> GetDailyCB(StockParameter stockParameter)
        {
            List<DailyStockDetailsEntity> dailyStockDetails = new List<DailyStockDetailsEntity>();
            List<StockCBEntity> cBEntities = new List<StockCBEntity>();

            StockStatementByDate statementByDate = new StockStatementByDate();
            dailyStockDetails = statementByDate.ProcessStockStatement(stockParameter);

            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            DataSet dataSetApproval = new DataSet();
            List<KeyValuePair<string, string>> _mastersqlParameters = new List<KeyValuePair<string, string>>();
            _mastersqlParameters.Add(new KeyValuePair<string, string>("@GCode", stockParameter.GCode));
            _mastersqlParameters.Add(new KeyValuePair<string, string>("@RCode", stockParameter.RCode));
            _mastersqlParameters.Add(new KeyValuePair<string, string>("@Date", stockParameter.FDate));
            dataSetApproval = manageSQLConnection.GetDataSetValues("GetApprovalStatus", _mastersqlParameters);

            var result = dailyStockDetails.GroupBy(a => a.GodownCode).Select(b => b.FirstOrDefault());
            foreach (var item in result)
            {
                StockCBEntity stockCB = new StockCBEntity();
                bool isValue = false;
                var newData = dailyStockDetails.Where(a => a.GodownCode == item.GodownCode).ToList();
                DataRow[] godownApproval = dataSetApproval.Tables[0].Select("GCode='"+ item.GodownCode + "'");
                DataRow[] regionApproval = dataSetApproval.Tables[1].Select("GCode='"+ item.GodownCode + "'");
                stockCB.RNAME = newData[0].RName;
                stockCB.TNCSName = newData[0].GName;
                stockCB.TNCSCapacity = newData[0].TNCSCapacity;
                stockCB.RStatus = GetApprovalStatus(regionApproval);
                stockCB.GStatus = GetApprovalStatus(godownApproval);
                stockCB.RRemark= GetApprovalRemark(regionApproval);
                stockCB.GRemark = GetApprovalRemark(godownApproval);
                foreach (var Details in newData)
                {
                    if (Details.ItemCode == "IT001")
                    {
                        isValue = true;
                        stockCB.PADDY_A = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT003")
                    {
                        isValue = true;
                        stockCB.PADDY_COMMON = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT009")
                    {
                        isValue = true;
                        stockCB.RAW_RICE_COMMON = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT012")
                    {
                        isValue = true;
                        stockCB.BOILED_RICE_COMMON = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT013")
                    {
                        isValue = true;
                        stockCB.SUGAR = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT014")
                    {
                        isValue = true;
                        stockCB.WHEAT = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT015")
                    {
                        isValue = true;
                        stockCB.RAVA = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT016")
                    {
                        isValue = true;
                        stockCB.MAIDA = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT018")
                    {
                        isValue = true;
                        stockCB.TOOR_DHALL = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT019")
                    {
                        isValue = true;
                        stockCB.URID_DHALL = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT020")
                    {
                        isValue = true;
                        stockCB.MAZOOR__DHALL = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT021")
                    {
                        isValue = true;
                        stockCB.GREEN_GRAM_DHALL = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT024")
                    {
                        isValue = true;
                        stockCB.GREEN_GRAM = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT025")
                    {
                        isValue = true;
                        stockCB.BENGAL_GRAM = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT026")
                    {
                        isValue = true;
                        stockCB.PALMOLIEN_OIL = GetCBQty(Details.PhycialBalance, 0);
                    }
                    else if (Details.ItemCode == "IT027")
                    {
                        isValue = true;
                        stockCB.SALT = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT029")
                    {
                        isValue = true;
                        stockCB.TEA = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT030")
                    {
                        isValue = true;
                        stockCB.KEROSENE = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT033")
                    {
                        isValue = true;
                        stockCB.RAW_RICE_A = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT034")
                    {
                        isValue = true;
                        stockCB.BOILED_RICE_A = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT041")
                    {
                        isValue = true;
                        stockCB.JUTE_TWINE = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT059")
                    {
                        isValue = true;
                        stockCB.PALMOLIEN_POUCH = GetCBQty(Details.PhycialBalance, 0);
                    }
                    else if (Details.ItemCode == "IT064")
                    {
                        isValue = true;
                        stockCB.CEMENT_IMPORTED = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT065")
                    {
                        isValue = true;
                        stockCB.ATTA = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT066")
                    {
                        isValue = true;
                        stockCB.CEMENT_REGULAR = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT112")
                    {
                        isValue = true;
                        stockCB.URID_DHALL_SPLIT = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT120")
                    {
                        isValue = true;
                        stockCB.Candian_Yellow_lentil_TD = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT121")
                    {
                        isValue = true;
                        stockCB.SALT_FF = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT123")
                    {
                        isValue = true;
                        stockCB.RAW_RICE_A_HULLING = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT124")
                    {
                        isValue = true;
                        stockCB.RAW_RICE_COM_HULLING = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT132")
                    {
                        isValue = true;
                        stockCB.YELLOW_LENTAL_US = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT133")
                    {
                        isValue = true;
                        stockCB.URAD_SQ = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT139")
                    {
                        isValue = true;
                        stockCB.BOILED_RICE_A_HULLING = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT140")
                    {
                        isValue = true;
                        stockCB.BOILED_RICE_C_HULLING = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT166")
                    {
                        isValue = true;
                        stockCB.LIARD_LENTIL_GREEN = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT175")
                    {
                        isValue = true;
                        stockCB.TUR_LEMON = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT176")
                    {
                        isValue = true;
                        stockCB.URAD_FAQ = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT190")
                    {
                        isValue = true;
                        stockCB.TUR_ARUSHA = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT191")
                    {
                        isValue = true;
                        stockCB.URID_DHALL_FAQ = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT192")
                    {
                        isValue = true;
                        stockCB.URID_DHALL_SQ = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT209")
                    {
                        isValue = true;
                        stockCB.AMMA_SALT_RFFIS = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT210")
                    {
                        isValue = true;
                        stockCB.AMMA_SALT_DFS = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT211")
                    {
                        isValue = true;
                        stockCB.AMMA_SALT_LSS = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT212")
                    {
                        isValue = true;
                        stockCB.AMMA_CEMENT = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT215")
                    {
                        isValue = true;
                        stockCB.AMMA_SALT_CIS = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT225")
                    {
                        isValue = true;
                        stockCB.OMR_HULLING = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT229")
                    {
                        isValue = true;
                        stockCB.BOILED_RICE_FORTIFIED = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT254")
                    {
                        isValue = true;
                        stockCB.Fortified_Rice_Kernels = GetCBQty(Details.PhycialBalance, 1);
                    }
                    else if (Details.ItemCode == "IT255")
                    {
                        isValue = true;
                        stockCB.FRK_Blended_Rice = GetCBQty(Details.PhycialBalance, 1);
                    }
                }
                if (isValue)
                {
                    cBEntities.Add(stockCB);
                }
            }
            return cBEntities;
        }

        public decimal GetCBQty(decimal qty, int type)
        {
            try
            {
                ManageReport report = new ManageReport();
                decimal svalue = Math.Round((type == 1 ? qty / 1000 : qty),3);
                return Convert.ToDecimal(report.DecimalformatForWeight(Convert.ToString(svalue)));
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return 0;
            }
        }

        public string GetApprovalStatus(DataRow[] dataRows)
        {
            try
            {
                if (dataRows.Count() > 0)
                {
                    return "Approved";
                }
                else
                {
                    return "Pending";
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return "Pending";
            }
            
        }
        public string GetApprovalRemark(DataRow[] dataRows)
        {
            try
            {
                if (dataRows.Count() > 0)
                {
                    return dataRows[0]["remarks"].ToString();// remarks;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
                return  string.Empty;
            }

        }
    }
}
