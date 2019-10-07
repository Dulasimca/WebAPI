using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace TNCSCAPI.ManageAllReports
{
    public class StockDeliveryOrderRegister
    {
        private string GName { get; set; }
        private string Regioncode { get; set; }
        private string FromDate { get; set; }
        private string ToDate { get; set; }
        ManageReport report = new ManageReport();

        public DataTable ManageDORegister(DataSet ds)
        {

            try
            {
                if (ds.Tables.Count > 1)
                {
                    DataTable ndt = ds.Tables[0].Clone();
                    var dataTableForDO = ds.Tables[0].DefaultView.ToTable(true, "Dono");
                    foreach (DataRow item in dataTableForDO.Rows)
                    {
                        DataRow[] dataRowForDODET = ds.Tables[0].Select("Dono='" + Convert.ToString(item["Dono"]) + "'");
                        DataRow[] dataRowForPayment = ds.Tables[1].Select("Dono='" + Convert.ToString(item["Dono"]) + "'");
                        int detCount = dataRowForDODET.Count();
                        int payCount = dataRowForPayment.Count();
                        if (payCount == 0)
                        {
                            foreach (DataRow dr in dataRowForDODET)
                            {
                                ndt.ImportRow(dr);
                                ndt.AcceptChanges();
                            }
                        }
                        else if (detCount >= payCount)
                        {
                            int i = 0;
                            foreach (DataRow dr in dataRowForDODET)
                            {
                                if (i < payCount)
                                {
                                    dr["Cheque_DD"] = Convert.ToString(dataRowForPayment[i]["Cheque_DD"]);
                                    dr["PaymentAmount"] = Convert.ToString(dataRowForPayment[i]["PaymentAmount"]);
                                    ndt.ImportRow(dr);
                                }
                                else
                                {
                                    ndt.ImportRow(dr);
                                }
                                ndt.AcceptChanges();
                                i++;
                            }
                        }
                        else if (payCount > detCount)
                        {
                            int i = 0;
                            foreach (DataRow dr in dataRowForDODET)
                            {
                                if (i < payCount)
                                {
                                    dr["Cheque_DD"] = Convert.ToString(dataRowForPayment[i]["Cheque_DD"]);
                                    dr["PaymentAmount"] = Convert.ToString(dataRowForPayment[i]["PaymentAmount"]);
                                    ndt.ImportRow(dr);
                                }
                                else
                                {
                                    ndt.ImportRow(dr);
                                }
                                ndt.AcceptChanges();
                                i++;
                            }
                            // i = i - 1;
                            if (payCount > i)
                            {
                                try
                                {
                                    
                                    for (int j = i; j >= payCount - i; j++)
                                    {
                                        DataRow dataRow = dataRowForDODET[0];
                                        dataRow["Totals"] = "0";
                                        dataRow["Scheme"] = "";
                                        dataRow["Commodity"] = "";
                                        dataRow["Netwt_Kgs"] = "0";
                                        dataRow["Rate_Rs"] = "0";
                                        dataRow["Itemamount"] = "0";
                                        dataRow["PreviousAmount"] = "0";
                                        dataRow["Adjusted"] = "0";
                                        dataRow["Balance"] = "0";
                                        dataRow["MarginAmount"] = "0";
                                        dataRow["Cheque_DD"] = Convert.ToString(dataRowForPayment[j]["Cheque_DD"]);
                                        dataRow["PaymentAmount"] = Convert.ToString(dataRowForPayment[j]["PaymentAmount"]);
                                        ndt.ImportRow(dataRow);
                                        ndt.AcceptChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AuditLog.WriteError("PayCount is more than Itemcount : " + ex.Message +" " + ex.StackTrace);
                                }
                                
                            }
                            //Totals,Scheme,Commodity,Netwt_Kgs,Rate_Rs,Itemamount,PreviousAmount,Adjusted,Balance,MarginAmount
                        }
                    }
                    return ndt;

                }
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" ManageDORegister :  " + ex.Message + " " + ex.StackTrace);
            }

            return null;
        }

        public void GenerateDeliveryOrderForRegister(CommonEntity entity)
        {
            string fPath = string.Empty, sPath = string.Empty, sFileName = string.Empty;
            string filePath = string.Empty;
            StreamWriter sw = null;
            try
            {
                GName = entity.dataTable.Rows[0]["Godown"].ToString();
                Regioncode = entity.dataTable.Rows[0]["Region"].ToString();
                sFileName = entity.GCode + GlobalVariable.StockDORegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                sPath = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(sPath);
                //delete file if exists
                filePath = sPath + "//" + sFileName + ".txt";
                report.DeleteFileIfExists(filePath);

                sw = new StreamWriter(filePath, true);

                WriteDORegForDateWise(sw, entity);
                // sw.WriteLine((char)12);
                List<DORegEntity> dORegEntities = new List<DORegEntity>();
                dORegEntities = report.ConvertDataTableToList<DORegEntity>(entity.dataTable);

                WriteDORegforSocityandScheme(sw, dORegEntities, entity);

                WriteDORegforItemandScheme(sw, dORegEntities, entity);

                WriteDORegforItem(sw, dORegEntities, entity);
                sw.Flush();
                //send mail to corresponding godown.

            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GenerateDeliveryOrderForRegister " + ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                sw.Close();
                sw = null;
                fPath = string.Empty; sFileName = string.Empty;
            }
        }

        /// <summary>
        /// Add header for Date wise report
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="entity">Common Entity</param>
        public void AddHeader(StreamWriter sw, string date)
        {
            sw.WriteLine("                                  TAMILNADU CIVIL SUPPLIES CORPORATION                                  Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine("                                          Delivery Order Details");
            sw.WriteLine(" ");
            sw.WriteLine("Delivery Order Date:" + report.FormatDate(date) + "           Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("S.No|  D.O.No   |   Total(Rs)     |  IssuerName   |Cheque/DD  | Pay.Amount(Rs)|   Scheme    |   Commodity  |Net.Wt(Kgs)| Rate(Rs)     | ITEM          |  Previous     |Other        |  Current     |  Margin    |");
            sw.WriteLine("    |           |                 |               |           |               |             |              |           |              | Amount        |   Balance     |Amount       |  Balance     |  Amount    |");
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("    |           |                 |               |           |               |             |              |           |              |               |               |             |              |            |");
        }

        /// <summary>
        /// Add header for  Socity, Scheme and  item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="entity">Common Entity</param>
        public void AddHeaderforAbstractSchemeandSociety(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                          Delivery Order Register Society Scheme and Rate Wise Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Delivery Order Date:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "           Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine("  SOCIETY                                     |ITEMS               |SCHEME           |Wt (Kgs/Nos)     |RATE             |AMOUNT              |");
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// Add header for  Scheme and  item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="entity">Common Entity</param>
        public void AddHeaderforAbstractItemandScheme(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("                          Delivery Order Register Commodity Scheme and Rate Wise Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("Delivery Order Date:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "           Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("-----------------------------------------------------------------------------------------------|");
            sw.WriteLine("ITEMS               |SCHEME           |Wt (Kgs/Nos)     |RATE             |AMOUNT              |");
            sw.WriteLine("-----------------------------------------------------------------------------------------------|");
        }

        /// <summary>
        /// Add header for item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="entity">Common Entity</param>
        public void AddHeaderforAbstractItem(StreamWriter sw, CommonEntity entity)
        {
            sw.WriteLine("   Delivery Order Register Commodity and Rate Wise Abstract          Report Date :   " + ManageReport.GetCurrentDate());
            sw.WriteLine(" ");
            sw.WriteLine(" ");
            sw.WriteLine("DO Date:" + report.FormatDate(entity.FromDate) + " - " + report.FormatDate(entity.Todate) + "  Godown : " + GName + "          Region :" + Regioncode);
            sw.WriteLine("-----------------------------------------------------------------------------|");
            sw.WriteLine("ITEMS               |Wt (Kgs/Nos)     |RATE             |AMOUNT              |");
            sw.WriteLine("-----------------------------------------------------------------------------|");
        }

        /// <summary>
        ///  Write text file for Date wise record.
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="entity">Common Entity</param>
        public void WriteDORegForDateWise(StreamWriter sw, CommonEntity entity)
        {
            int iCount = 10;
            var distinctDate = entity.dataTable.DefaultView.ToTable(true, "DeliveryOrderDate");
            //Date wise DO report
            int i = 1;
            string sIssuer = string.Empty;
            bool CheckRepeatValue = false;
            string sDoNo = string.Empty;
            foreach (DataRow dateValue in distinctDate.Rows)
            {
                iCount = 11;
                string sDoNo1 = string.Empty;
                DataRow[] datas = entity.dataTable.Select("DeliveryOrderDate='" + dateValue["DeliveryOrderDate"] + "'");
                AddHeader(sw, Convert.ToString(dateValue["DeliveryOrderDate"]));
                foreach (DataRow dr in datas)
                {

                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeader(sw, Convert.ToString(dateValue["DeliveryOrderDate"]));
                    }
                    sDoNo1 = dr["Dono"].ToString();
                    if (sDoNo == sDoNo1)
                    {
                        CheckRepeatValue = true;
                    }
                    else
                    {
                        CheckRepeatValue = false;
                        sDoNo = sDoNo1;
                    }
                    sIssuer = Convert.ToString(dr["To_Whom_Issued"]).Trim();
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? i.ToString() : " ", 4, 2));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? sDoNo1 : " ", 11, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? report.Decimalformat(Convert.ToString(dr["Totals"])) : "-", 17, 1));
                    sw.Write(report.StringFormat(CheckRepeatValue == false ? sIssuer : " ", 15, 2));
                    sw.Write(report.StringFormat(dr["Cheque_DD"].ToString(), 11, 2));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["PaymentAmount"].ToString()), 15, 1));
                    sw.Write(report.StringFormat(dr["Scheme"].ToString(), 13, 2));
                    sw.Write(report.StringFormat(dr["Commodity"].ToString(), 14, 2));
                    sw.Write(report.StringFormat(report.DecimalformatForWeight(dr["Netwt_Kgs"].ToString()), 11, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["Rate_Rs"].ToString()), 14, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["Itemamount"].ToString()), 15, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["PreviousAmount"].ToString()), 15, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["Adjusted"].ToString()), 13, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["Balance"].ToString()), 14, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(dr["MarginAmount"].ToString()), 12, 1));
                    sw.WriteLine("");
                    int addedLines = 0;
                    if (!CheckRepeatValue)
                    {
                        addedLines = report.AddMoreContent(sw, sIssuer, 15, 35);
                    }

                    sw.WriteLine("    |           |                 |               |           |               |             |              |           |              |               |               |             |              |            |");
                    iCount = iCount + 2 + addedLines;
                    i = CheckRepeatValue == false ? i + 1 : i;
                }
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
        }

        /// <summary>
        ///  Write text file for Socity, Scheme and  item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="dORegEntities">DOReg entity</param>
        /// <param name="entity">Common Entity</param>
        public void WriteDORegforSocityandScheme(StreamWriter sw, List<DORegEntity> dORegEntities, CommonEntity entity)
        {
            int iCount = 11;
            // Gets the group by values based on ths column To_Whom_Issued, Commodity,Scheme
            var myResult = from a in dORegEntities
                           group a by new { a.To_Whom_Issued, a.Commodity, a.Scheme, a.Rate_Rs } into gValue
                           select new
                           {
                               Netwt_Kgs = gValue.Sum(s => s.Netwt_Kgs),
                               Itemamount = gValue.Sum(s => s.Itemamount),
                               GroupByNames = gValue.Key
                           };
            AddHeaderforAbstractSchemeandSociety(sw, entity);

            foreach (var item in myResult)
            {
                if (iCount >= 50)
                {
                    //Add header again
                    iCount = 11;
                    sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractSchemeandSociety(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.To_Whom_Issued, 46, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 20, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Scheme, 17, 2));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 17, 1));
                sw.Write(report.StringFormat(report.Decimalformat(item.GroupByNames.Rate_Rs.ToString()), 17, 1));
                sw.Write(report.StringFormat(report.Decimalformat(item.Itemamount.ToString()), 20, 1));
                iCount++;
                sw.WriteLine("");
            }
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        ///  Write text file for item and scheme wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="dORegEntities">DOReg entity</param>
        /// <param name="entity">Common Entity</param>
        public void WriteDORegforItemandScheme(StreamWriter sw, List<DORegEntity> dORegEntities, CommonEntity entity)
        {
            int iCount = 11;
            // Gets the group by values based on ths column Commodity,Scheme
            var myResultItemandScheme = from a in dORegEntities
                                        group a by new { a.Commodity, a.Scheme, a.Rate_Rs } into gValue
                                        select new
                                        {
                                            Netwt_Kgs = gValue.Sum(s => s.Netwt_Kgs),
                                            Itemamount = gValue.Sum(s => s.Itemamount),
                                            GroupByNames = gValue.Key
                                        };

            AddHeaderforAbstractItemandScheme(sw, entity);
            foreach (var item in myResultItemandScheme)
            {
                if (iCount >= 50)
                {
                    //Add header again
                    iCount = 11;
                    sw.WriteLine("-----------------------------------------------------------------------------------------------|");
                    sw.WriteLine((char)12);
                    AddHeaderforAbstractItemandScheme(sw, entity);
                }
                sw.Write(report.StringFormat(item.GroupByNames.Commodity, 20, 2));
                sw.Write(report.StringFormat(item.GroupByNames.Scheme, 17, 2));
                sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 17, 1));
                sw.Write(report.StringFormat(report.Decimalformat(item.GroupByNames.Rate_Rs.ToString()), 17, 1));
                sw.Write(report.StringFormat(report.Decimalformat(item.Itemamount.ToString()), 20, 1));
                iCount++;
                sw.WriteLine("");
            }
            sw.WriteLine("-----------------------------------------------------------------------------------------------|");
            sw.WriteLine((char)12);
        }

        /// <summary>
        /// Write text file for item wise abstract data
        /// </summary>
        /// <param name="sw">Text Streamwriter</param>
        /// <param name="dORegEntities">DOReg entity</param>
        /// <param name="entity">Common Entity</param>
        public void WriteDORegforItem(StreamWriter sw, List<DORegEntity> dORegEntities, CommonEntity entity)
        {
            try
            {
                int iCount = 11;
                // Gets the group by values based on ths column Commodity,Scheme
                var myResultItem = from a in dORegEntities
                                   group a by new { a.Commodity, a.Rate_Rs } into gValue
                                   select new
                                   {
                                       Netwt_Kgs = gValue.Sum(s => s.Netwt_Kgs),
                                       Itemamount = gValue.Sum(s => s.Itemamount),
                                       GroupByNames = gValue.Key
                                   };

                double dAmount = 0;
                AddHeaderforAbstractItem(sw, entity);
                foreach (var item in myResultItem)
                {
                    if (iCount >= 50)
                    {
                        //Add header again
                        iCount = 11;
                        sw.WriteLine("-----------------------------------------------------------------------------|");
                        sw.WriteLine((char)12);
                        AddHeaderforAbstractItem(sw, entity);
                    }
                    sw.Write(report.StringFormat(item.GroupByNames.Commodity, 20, 2));
                    sw.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 17, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(item.GroupByNames.Rate_Rs.ToString()), 17, 1));
                    sw.Write(report.StringFormat(report.Decimalformat(item.Itemamount.ToString()), 20, 1));
                    dAmount = dAmount + item.Itemamount;
                    iCount++;
                    sw.WriteLine("");
                }
                // Add toal values
                sw.WriteLine("-----------------------------------------------------------------------------|");
                sw.Write("                                        Total Amount    |");
                sw.Write(report.StringFormat(report.Decimalformat(dAmount.ToString()), 20, 1));
                sw.WriteLine("");
                sw.WriteLine("-----------------------------------------------------------------------------|");
                sw.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
            }

        }


    }

    public class DORegEntity
    {
        public string Region { get; set; }
        public string Godown { get; set; }
        public DateTime DeliveryOrderDate { get; set; }
        public string Dono { get; set; }
        public double Totals { get; set; }
        public string To_Whom_Issued { get; set; }
        public string Cheque_DD { get; set; }
        public double PaymentAmount { get; set; }
        public string Scheme { get; set; }
        public string Commodity { get; set; }
        public double Netwt_Kgs { get; set; }
        public double Rate_Rs { get; set; }
        public double Itemamount { get; set; }
        public double PreviousAmount { get; set; }
        public double Adjusted { get; set; }
        public double Balance { get; set; }
        public double MarginAmount { get; set; }
    }

    public class CommonEntity
    {
        public DataSet dataSet { get; set; }
        public string GCode { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public string UserName { get; set; }
        public string GName { get; set; }
        public string RName { get; set; }
        public DataTable dataTable { get; set; }
    }

}
