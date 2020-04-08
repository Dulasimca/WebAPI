using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.DataTransfer;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageIssuesAbstractPrint
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockIssuesEntity"></param>
        public bool GenerateAbstractPrint(DataSet dataSet, GatePassCommonEntity gatePassCommon)
        {
            // AuditLog.WriteError("GeneratestockIssuesEntityRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            bool isPrint = false;
            try
            {
                fileName = gatePassCommon.GCode + GlobalVariable.IssueGatePassFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + gatePassCommon.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                streamWriter = new StreamWriter(filePath, true);
                List<GatePassIssuesEntity> stockIssueList = new List<GatePassIssuesEntity>();
                stockIssueList = report.ConvertDataTableToList<GatePassIssuesEntity>(dataSet.Tables[0]);

                AddDocHeaderForIssues(streamWriter, stockIssueList, gatePassCommon);
                AddDetails(streamWriter, stockIssueList);
                AddFooter(streamWriter, stockIssueList);
                isPrint = true;
            }
            catch (Exception ex)
            {
                isPrint = false;
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                streamWriter.Flush();
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
            return isPrint;
        }

        /// <summary>
        /// Add header for document receipt
        /// </summary>
        /// <param name="streamWriter">Stream writer to write the text file.</param>
        /// <param name="stockIssuesEntity"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForIssues(StreamWriter streamWriter, List<GatePassIssuesEntity> stockIssuesEntity, GatePassCommonEntity gatePassCommon)
        {
            var distReceiver = stockIssuesEntity.GroupBy(o => new { o.SINo, o.ReceivorName })
                                   .Select(o => o.FirstOrDefault());

            string receiverdetais = string.Empty;
            foreach (var item in distReceiver)
            {
                receiverdetais = receiverdetais + item.SINo + "-" + item.ReceivorName.Trim() + ",";
            }
            receiverdetais = receiverdetais.TrimEnd(',');

            streamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|                                          TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.Write("|                                              ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(gatePassCommon.RName, 53, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                             |");
            if (gatePassCommon.Type == 2)
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE - ISSUE MEMO                Abstract Duplicate Print       |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE - ISSUE MEMO                Abstract Print                 |");
            }
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|   GATE PASS NUMBER : ");
            streamWriter.Write(report.StringFormatWithoutPipe(gatePassCommon.GatePassNo, 27, 2));
            streamWriter.Write("GATE PASS DATE: ");
            streamWriter.Write(report.StringFormatWithoutPipe(ManageReport.GetCurrentDate(), 15, 2));
            streamWriter.Write("TIME : ");
            streamWriter.Write(report.StringFormat(report.GetCurrentTime(DateTime.Now), 20, 2));
            streamWriter.WriteLine(" ");

            streamWriter.Write("|   ISSUING GODOWN   : ");
            streamWriter.Write(report.StringFormatWithoutPipe(gatePassCommon.GName, 27, 2));
            streamWriter.Write("Doc DATE      : ");
            streamWriter.Write(report.StringFormat(report.FormatDate(stockIssuesEntity[0].SIDate.ToString()), 43, 2));
            //streamWriter.Write(report.StringFormatWithoutPipe((stockIssuesEntity[0].IssueRegularAdvance.ToUpper() == "R" ? "REGULAR" : "ADVANCE"), 9, 2));
            //streamWriter.Write(report.StringFormat(stockIssuesEntity[0].IRelates, 18, 2));
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("||                                                                                                            |");
            streamWriter.WriteLine("|| Issuer Details:                                                                                            |");
            streamWriter.Write("||  ");
            streamWriter.Write(report.StringFormat(receiverdetais, 106, 2));
            streamWriter.WriteLine(" ");
            report.AddMoreContentForGatePass(streamWriter, receiverdetais, 106);
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
            streamWriter.WriteLine("||SNo|  STACK NO  |    COMMODITY                  |  SCHEME      |UNIT WEIGHT  |NO.OFUNITS|   NET Wt/Nos|MOI% |");
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddDetails(StreamWriter streamWriter, List<GatePassIssuesEntity> stockIssuesEntity)
        {
            int i = 0;
            int units = 0;
            double netKgs = 0;
            var resultSet = (from d in stockIssuesEntity
                             orderby d.ITName ascending
                             group d by new { d.ITName, d.TStockNo, d.SchemeName, d.PName, d.Moisture } into groupedData
                             select new
                             {
                                 Netwt_Kgs = groupedData.Sum(s => s.Nkgs),
                                 No_Bags = groupedData.Sum(s => s.NoPacking),
                                 GroupByNames = groupedData.Key
                             });

            foreach (var item in resultSet)
            {
                i = i + 1;
                if (item.GroupByNames.TStockNo.ToUpper() != "TOTAL")
                {
                    streamWriter.Write("||");
                    streamWriter.Write(report.StringFormat(i.ToString(), 3, 2));
                    streamWriter.Write(report.StringFormat(item.GroupByNames.TStockNo, 12, 2));
                    streamWriter.Write(report.StringFormat(item.GroupByNames.ITName, 31, 2));
                    streamWriter.Write(report.StringFormat(item.GroupByNames.SchemeName, 14, 2));
                    streamWriter.Write(report.StringFormat(item.GroupByNames.PName, 13, 2));
                    streamWriter.Write(report.StringFormat(item.No_Bags.ToString(), 10, 1));
                    streamWriter.Write(report.StringFormat(report.DecimalformatForWeight(item.Netwt_Kgs.ToString()), 13, 1));
                    streamWriter.Write(report.StringFormat(item.GroupByNames.Moisture.ToString(), 5, 1));
                    streamWriter.WriteLine(" ");
                    units = units + item.No_Bags;
                    netKgs = netKgs + Convert.ToDouble(item.Netwt_Kgs);
                }
            }
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
            streamWriter.WriteLine("||                                                               |Total        |" + report.StringFormatWithoutPipe(units.ToString(), 9, 1) + "|" + report.StringFormatWithoutPipe(report.DecimalformatForWeight(netKgs.ToString()), 12, 1) + "|     |");
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");

        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddFooter(StreamWriter streamWriter, List<GatePassIssuesEntity> stockIssuesEntity)
        {
            streamWriter.WriteLine("| LORRY NO      :" + report.StringFormatWithoutPipe(report.ConvertToUpper(stockIssuesEntity[0].LorryNo), 17, 2) + "TC NAME     : " + report.StringFormatWithoutPipe(report.ConvertToUpper(stockIssuesEntity[0].TransporterName), 60, 2) + "|");
           // streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI1, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI2, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI3, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI4, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI5, 108, 2) + "|");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|          Signature of the representative.                                     GODOWN INCHARGE               |");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);
        }

        public string GetDocumentNumber(List<GatePassIssuesEntity> gatePassIssues)
        {
            string documents = string.Empty;
            var result = gatePassIssues.GroupBy(a => a.SINo).Select(g => g.First()).ToList();
            foreach (var item in result)
            {
                documents += item.SINo + ",";
            }
            return documents.TrimEnd(',');
        }

        public void ProcessDataToGPS(DataSet dataSet, GatePassCommonEntity gatePassCommon)
        {
            List<GatePassIssuesEntity> stockIssueList = new List<GatePassIssuesEntity>();
            stockIssueList = report.ConvertDataTableToList<GatePassIssuesEntity>(dataSet.Tables[0]);
            InsertForGPS(stockIssueList, gatePassCommon);
        }

        public void InsertForGPS(List<GatePassIssuesEntity> gatePassIssues, GatePassCommonEntity gatePassCommon)
        {
            try
            {
                ManageDataTransfer dataTransfer = new ManageDataTransfer();
                var distvalue = gatePassIssues.FirstOrDefault();
                if (GPSInsert(distvalue.IssueType))
                {
                    ManageReport report = new ManageReport();
                    DataTransferEntity dataTransferEntity = new DataTransferEntity
                    {
                        GCode = gatePassCommon.GCode,
                        RCode = distvalue.IssuingCode,
                        DocType = 2,
                        TripType = 2,
                        G2GStatus = 4,
                        GPSStatus = 0,
                        DocDate = report.FormatDirectDate(distvalue.SIDate),
                        DocNumber = GetDocumentNumber(gatePassIssues)
                    };
                    dataTransfer.InsertDataTransfer(dataTransferEntity);
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }

        }

        public bool GPSInsert(string Value)
        {
            bool isInsert = false;
            try
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();

                List<KeyValuePair<string, string>> sqlParameters1 = new List<KeyValuePair<string, string>>();
                sqlParameters1.Add(new KeyValuePair<string, string>("@TypeValue", Value));
                var result1 = manageSQLConnection.GetDataSetValues("GetDataInsertTOGPS", sqlParameters1);
                if (result1.Tables[0].Rows.Count > 0)
                {
                    isInsert = true;
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GPSInsert : " + ex.Message);
            }
            return isInsert;
        }
    }
    public class GatePassIssuesEntity
    {
        public string SINo { get; set; }
        public DateTime SIDate { get; set; }
        public string ReceivorName { get; set; }
        public string IRelates { get; set; }
        public string TransporterName { get; set; }
        public string LorryNo { get; set; }
        public string TStockNo { get; set; }
        public double Moisture { get; set; }
        public string SchemeName { get; set; }
        public string ITName { get; set; }
        public string PName { get; set; }
        public int NoPacking { get; set; }
        public double GKgs { get; set; }
        public double Nkgs { get; set; }
        public string IssueRegularAdvance { get; set; }
        public string IssueType { get; set; }
        public string IssuingCode { get; set; }
    }

    public class GatePassCommonEntity
    {
        public string GName { get; set; }
        public string GCode { get; set; }
        public string RName { get; set; }
        public string DocNumber { get; set; }
        public string GatePassNo { get; set; }
        public string UserID { get; set; }
        public int Type { get; set; }
    }
}
