﻿using System;
using System.IO;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentIssues
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockIssuesEntity"></param>
        public void GenerateIssues(DocumentStockIssuesEntity stockIssuesEntity)
        {
            // AuditLog.WriteError("GeneratestockIssuesEntityRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            bool isDuplicate = false;
            try
            {
                fileName = stockIssuesEntity.IssuingCode + GlobalVariable.DocumentIssueFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + stockIssuesEntity.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                isDuplicate = stockIssuesEntity.Loadingslip == null ? false : stockIssuesEntity.Loadingslip.ToUpper() == "Y" ? true : false;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForIssues(streamWriter, stockIssuesEntity, isDuplicate);
                AddDetails(streamWriter, stockIssuesEntity);
                AddDODetails(streamWriter, stockIssuesEntity);
                AddFooter(streamWriter, stockIssuesEntity);

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                streamWriter.Flush();
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
        }

        /// <summary>
        /// Add header for document receipt
        /// </summary>
        /// <param name="streamWriter">Stream writer to write the text file.</param>
        /// <param name="stockIssuesEntity"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForIssues(StreamWriter streamWriter, DocumentStockIssuesEntity stockIssuesEntity, bool isDuplicate = false)
        {
            streamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|                                          TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.Write("|                                              ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(stockIssuesEntity.RegionName, 53, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|                                                                                                             |");
            if (isDuplicate)
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE - ISSUE MEMO                DUPLICATE COPY                 |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE - ISSUE MEMO                                               |");
            }
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|ISSUE MEMO NO  :   ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockIssuesEntity.SINo, 21, 2));
            streamWriter.Write("DATE: ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockIssuesEntity.SIDate.ToString()), 12, 2));
            streamWriter.Write("TIME:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.GetCurrentTime(DateTime.Now), 14, 2));
            streamWriter.Write(report.StringFormatWithoutPipe((stockIssuesEntity.IssueRegularAdvance.ToUpper() == "R" ? "REGULAR" : "ADVANCE"), 8, 2));
            streamWriter.Write(report.StringFormat(stockIssuesEntity.IRelates, 20, 2));
            streamWriter.WriteLine(" ");

            streamWriter.Write("|ISSUING GODOWN :   ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockIssuesEntity.GodownName, 21, 2));
            streamWriter.Write(report.StringFormatWithoutPipe("TO WHOM ISSUED:", 15, 1));
            streamWriter.Write(report.StringFormatWithoutPipe(stockIssuesEntity.ReceiverName, 51, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            try
            {
                streamWriter.Write("|                                         ");
                streamWriter.Write(report.StringFormatWithoutPipe("ISSUER CODE  :", 15, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(stockIssuesEntity.IssuerCode, 51, 2));
                streamWriter.Write("|");
                streamWriter.WriteLine(" ");
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
            streamWriter.WriteLine("||SNo|  STACK NO  |    COMMODITY                  |  SCHEME      |UNIT WEIGHT  |NO.OFUNITS|   NET Wt/Nos|MOI% |");
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddDetails(StreamWriter streamWriter, DocumentStockIssuesEntity stockIssuesEntity)
        {
            int i = 0;
            int units = 0;
            double netKgs = 0;
            foreach (var item in stockIssuesEntity.IssueItemList)
            {
                i = i + 1;
                if (item.TStockNo.ToUpper() != "TOTAL")
                {
                    streamWriter.Write("||");
                    streamWriter.Write(report.StringFormat(i.ToString(), 3, 2));
                    streamWriter.Write(report.StringFormat(item.TStockNo, 12, 2));
                    streamWriter.Write(report.StringFormat(item.CommodityName, 31, 2));
                    streamWriter.Write(report.StringFormat(item.SchemeName, 14, 2));
                    streamWriter.Write(report.StringFormat(item.PackingName, 13, 2));
                    streamWriter.Write(report.StringFormat(item.NoPacking.ToString(), 10, 1));
                    streamWriter.Write(report.StringFormat(report.DecimalformatForWeight(item.Nkgs.ToString()), 13, 1));
                    streamWriter.Write(report.StringFormat(item.Moisture.ToString(), 5, 1));
                    streamWriter.WriteLine(" ");
                    units = units + item.NoPacking;
                    netKgs = netKgs + Convert.ToDouble(item.Nkgs);
                }
            }
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");
            streamWriter.WriteLine("||                                                               |Total        |" + report.StringFormatWithoutPipe(units.ToString(), 9, 1) + "|" + report.StringFormatWithoutPipe(report.DecimalformatForWeight(netKgs.ToString()), 12, 1) + "|     |");
            streamWriter.WriteLine("||------------------------------------------------------------------------------------------------------|-----|");

        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddDODetails(StreamWriter streamWriter, DocumentStockIssuesEntity stockIssuesEntity)
        {
            int i = 0;
            streamWriter.WriteLine("||-----------------------------------------------------------------|                                          |");
            streamWriter.WriteLine("|| DELIVERY ORDER      |  ISSUE/TRUCK MEMO   |     GATE PASS       |                                          |");
            streamWriter.WriteLine("|| NUMBER   | DATE     | NUMBER   | DATE     | NUMBER   | DATE     |                                          |");
            streamWriter.WriteLine("||-----------------------------------------------------------------|                                          |");
            foreach (var item in stockIssuesEntity.SIDetailsList)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(report.StringFormat(item.DNo, 10, 2));
                streamWriter.Write(report.StringFormat(report.FormatIndianDate(item.DDate.ToString()), 10, 2));
                streamWriter.Write(report.StringFormat(stockIssuesEntity.SINo, 10, 2));
                streamWriter.Write(report.StringFormat(report.FormatIndianDate(stockIssuesEntity.SIDate.ToString()), 10, 2));
                streamWriter.Write("          |          |                                          |");
                streamWriter.WriteLine(" ");
            }
            streamWriter.WriteLine("||-----------------------------------------------------------------|                                          |");
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");

        }


        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddFooter(StreamWriter streamWriter, DocumentStockIssuesEntity stockIssuesEntity)
        {
            streamWriter.WriteLine("| LORRY NO      :" + report.StringFormatWithoutPipe(report.ConvertToUpper(stockIssuesEntity.LorryNo), 17, 2) + "TC NAME     : " + report.StringFormatWithoutPipe(report.ConvertToUpper(stockIssuesEntity.TransporterName), 60, 2) + "|");
          //  streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI1, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI2, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI3, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI4, 108, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI5, 108, 2) + "|");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|          Signature of the representative.                                     GODOWN INCHARGE               |");
           // streamWriter.WriteLine("|          Sign. of the Authorised Person.                                     GODOWN INCHARGE                |");
            streamWriter.WriteLine("|                                                                                                             |");
            streamWriter.WriteLine("|REMARKS                                                                                                      |");
            streamWriter.WriteLine("|   " + report.StringFormatWithoutPipe(stockIssuesEntity.Remarks, 105, 2) + "|");
            report.AddMoreContent(streamWriter, stockIssuesEntity.Remarks, 105, 3);
            streamWriter.WriteLine("|-------------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);
        }
    }
}
