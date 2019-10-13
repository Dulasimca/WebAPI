using System;
using System.Data;
using System.IO;

namespace TNCSCAPI.ManageAllReports.Transfer
{
    public class ManageTruckToRegion
    {
        ManageReport report = new ManageReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateTruckToRegion(CommonEntity entity)
        {

            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.TruckToRegionFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                TruckToRegionWiseReport(streamWriter, entity);

                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="date"></param>
        public void AddHeader(StreamWriter sw, CommonEntity entity, int pageno)
        {
            sw.WriteLine("        TAMILNADU CIVIL SUPPLIES CORPORATION          " + entity.RName);
            sw.WriteLine("        Truck Memo Region Wise Details of  Godown: : " + entity.GName);
            sw.WriteLine("        Date:" + report.FormatDate(entity.FromDate) + "  To : " + report.FormatDate(entity.Todate) + "            PageNo : " + pageno.ToString());
            sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine("SNO  T.MEMO NO   DATE        RECEIVED GODOWN       REGION         Commodity       SCHEME       BAGS  NET Weight(Kgs) Doc.no/Lorry NO");
            sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void TruckToRegionWiseReport(StreamWriter sw, CommonEntity entity)
        {
            int count = 10;

            int i = 1;
            string doNo = string.Empty;
            string fromWhomRcd = string.Empty;
            bool isDataAvailable = false;
            decimal Total = 0;
            decimal GrandTotal = 0;
            int pageno = 1;
            count = 8;
            bool isfirst = true;
            try
            {
                // dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, new string[] { "Coop", "Comodity" });
                var dateList = entity.dataSet.Tables[0].DefaultView.ToTable(true, "DepositorName");
                AddHeader(sw, entity, pageno);
                foreach (DataRow depdata in dateList.Rows)
                {
                    Total = 0;
                    GrandTotal = 0;
                    isDataAvailable = true;
                    isfirst = true;
                    string PackingType = string.Empty;
                    string depositor = string.Empty;
                    depositor = Convert.ToString(depdata["DepositorName"]);
                    DataRow[] data = entity.dataSet.Tables[0].Select("DepositorName='" + depositor + "'");
                    var distinctCommodity = report.ConvertDataRowToTable(data, entity.dataSet.Tables[0]).DefaultView.ToTable(true, "ITDescription");

                    foreach (DataRow item in distinctCommodity.Rows)
                    {
                        Total = 0;
                        DataRow[] ndata = entity.dataSet.Tables[0].Select("DepositorName='" + depositor + "' and ITDescription='" + Convert.ToString(item["ITDescription"]) + "'");
                        foreach (DataRow row in ndata)
                        {

                            if (count >= 50)
                            {
                                //Add header again
                                pageno++;
                                count = 8;
                                sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
                                sw.WriteLine((char)12);
                                AddHeader(sw, entity, pageno);
                            }
                            PackingType = row["PBWeight"].ToString(); //PBWeight
                            sw.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 1));
                            sw.Write(report.StringFormatWithoutPipe(row["STNo"].ToString(), 11, 2));
                            sw.Write(report.StringFormatWithoutPipe(report.FormatDirectDate(row["STDate"].ToString()), 10, 2) + "  ");
                            sw.Write(report.StringFormatWithoutPipe(isfirst == true ? row["DepositorName"].ToString() : " ", 20, 2));
                            sw.Write(report.StringFormatWithoutPipe(isfirst == true ? row["RGNAME"].ToString() : " ", 15, 2));
                            sw.Write(report.StringFormatWithoutPipe(row["ITDescription"].ToString(), 15, 1));
                            sw.Write(report.StringFormatWithoutPipe(row["SCName"].ToString(), 11, 1));
                            sw.Write(report.StringFormatWithoutPipe(row["NoPacking"].ToString(), 6, 2));
                            sw.Write(report.StringFormatWithoutPipe(PackingType.ToUpper() == "NOS" ? row["Nkgs"].ToString() : report.DecimalformatForWeight(row["Nkgs"].ToString()), 15, 1));
                            sw.Write(report.StringFormatWithoutPipe(row["LNo"].ToString(), 21, 2));
                            sw.WriteLine("");
                            Total += !string.IsNullOrEmpty(Convert.ToString(row["Nkgs"])) ? Convert.ToDecimal(row["Nkgs"].ToString()) : 0;
                            i = i + 1;
                            count++;
                            isfirst = false;
                        }
                        sw.Write(report.StringFormatWithoutPipe("", 4, 1));
                        sw.Write(report.StringFormatWithoutPipe("", 11, 2));
                        sw.Write(report.StringFormatWithoutPipe("", 10, 2) + "  ");
                        sw.Write(report.StringFormatWithoutPipe(" ", 20, 2));
                        sw.Write(report.StringFormatWithoutPipe(" ", 15, 2));
                        sw.Write(report.StringFormatWithoutPipe("", 10, 1));
                        sw.Write(report.StringFormatWithoutPipe("Total", 16, 1));
                        sw.Write(report.StringFormatWithoutPipe("", 6, 2));
                        sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(Total.ToString()), 15, 1));
                        sw.WriteLine("");
                        count++;
                        GrandTotal += Total;
                    }

                    sw.Write(report.StringFormatWithoutPipe("", 4, 1));
                    sw.Write(report.StringFormatWithoutPipe("", 11, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 10, 2) + "  ");
                    sw.Write(report.StringFormatWithoutPipe(" ", 20, 2));
                    sw.Write(report.StringFormatWithoutPipe(" ", 15, 2));
                    sw.Write(report.StringFormatWithoutPipe("", 10, 1));
                    sw.Write(report.StringFormatWithoutPipe("Grand Total", 16, 1));
                    sw.Write(report.StringFormatWithoutPipe("", 6, 2));
                    sw.Write(report.StringFormatWithoutPipe(report.DecimalformatForWeight(GrandTotal.ToString()), 15, 1));
                    sw.WriteLine("");
                    count++;
                }
                if (!isDataAvailable)
                {
                    sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine((char)12);
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
