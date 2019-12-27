﻿using System;
using System.Collections.Generic;
using System.IO;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentReceipt
    {
        ManageReport report = new ManageReport();
        public void GenerateReceipt(DocumentStockReceiptList stockReceipt)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            bool isDuplicate = false;
            try
            {
                fileName = stockReceipt.ReceivingCode + GlobalVariable.DocumentReceiptFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + stockReceipt.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
              //  isDuplicate = ReceiptId == "0" ? false : true;
                isDuplicate = stockReceipt.UnLoadingSlip==null ? false: stockReceipt.UnLoadingSlip.ToUpper() == "Y" ? true : false;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForReceipt(streamWriter, stockReceipt, isDuplicate);
                AddDetails(streamWriter, stockReceipt);
                AddFooter(streamWriter, stockReceipt);
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
        /// <param name="stockReceipt"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForReceipt(StreamWriter streamWriter, DocumentStockReceiptList stockReceipt, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.Write("|                                           ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(stockReceipt.RegionName, 53, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            if(isDuplicate)
            {
            streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT            DUPLICATE COPY              |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT                                        |");
            }
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|ACKNOWLEDGEMENT NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.SRNo, 21, 2));
            streamWriter.Write("ALLOTMENT/RELEASE ORDER: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.OrderNo +" " + stockReceipt.PAllotment, 16, 2));
            streamWriter.Write("GATE PASS : ");
            streamWriter.Write(report.StringFormatWithoutPipe("", 10, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|              DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.SRDate.ToString()), 21, 2));
            streamWriter.Write(report.StringFormatWithoutPipe("DATE: ",25,1));
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.OrderDate.ToString()), 38, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|PERIOD OF ALLOTMENT:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.SRDate.ToString()), 30, 2));
            streamWriter.Write("Transaction Type: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.TransactionName, 36, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|RECEIVING GODOWN:");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.GodownName, 30, 2));
            streamWriter.Write("DEPOSITOR'S NAME:");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.DepositorName, 40, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("||--------------------------------------------------------------------------------------------------------||");
            streamWriter.WriteLine("||SNo |STACK NO   |COMMODITY           | SCHEME       |UNIT WEIGHT  |NO.OF |  Gross        NET   |% OF    ||");
            streamWriter.WriteLine("||    |           |                    |              |             |  UNIT|   WEIGHT in Kgs/NOs |MOISTURE||");
            streamWriter.WriteLine("||--------------------------------------------------------------------------------------------------------||");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddDetails(StreamWriter streamWriter, DocumentStockReceiptList stockReceipt)
        {
            int i = 0;
            foreach (var item in stockReceipt.ItemList)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(report.StringFormat(i.ToString(), 4, 2));
                streamWriter.Write(report.StringFormat(item.TStockNo, 11, 2));
                streamWriter.Write(report.StringFormat(item.CommodityName, 20, 2));
                streamWriter.Write(report.StringFormat(item.SchemeName, 14, 2));
                streamWriter.Write(report.StringFormat(item.PackingName, 13, 2));
                streamWriter.Write(report.StringFormat(item.NoPacking.ToString(), 6, 1));
                streamWriter.Write(report.StringFormat(item.GKgs.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(item.Nkgs.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(item.Moisture.ToString(), 8, 1) +"|");
                streamWriter.WriteLine(" ");
            }
           
        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddFooter(StreamWriter streamWriter, DocumentStockReceiptList stockReceipt)
        {
            //GetDate()
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            DateTime dateTime = manageSQL.GetSRTime(stockReceipt.SRNo);
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|T.MEMO/IN.NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.TruckMemoNo, 19, 2));
            streamWriter.Write("LORRY NO      : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.LNo, 14, 2));
            streamWriter.Write("TC NAME       : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.TransporterName, 25, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|T.MEMO/IN.DT:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.TruckMemoDate.ToString()), 19, 2));
            streamWriter.Write("LORRY FROM    : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.LFrom, 56, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.Write("|MODE OF WEIGHMENT: ");
            streamWriter.Write(report.StringFormatWithoutPipe(GetWTCode(stockReceipt), 13, 2));
            streamWriter.Write("WAGON NO      : ");
            streamWriter.Write(report.StringFormatWithoutPipe("-", 14, 2));
            streamWriter.Write("RR NO         : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.MTransport, 25, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI1, 105, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI2, 105, 2) + "|");
            streamWriter.WriteLine("|" + report.StringFormatWithoutPipe(GlobalVariable.FSSAI3, 105, 2) + "|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|  Sign. of the Authorised Person.                                            GODOWN INCHARGE              |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|REMARKS                                                                                                   |");
            streamWriter.WriteLine("|   "+ report.StringFormatWithoutPipe(stockReceipt.Remarks, 102, 2)+"|");
            report.AddMoreContent(streamWriter, stockReceipt.Remarks, 102, 3);
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            string receiptDateTime = stockReceipt.SRDate + " "+ report.GetCurrentTime(dateTime);
            streamWriter.WriteLine(" Prepared DateTime:"+ receiptDateTime + "       Printing DateTime:"+DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));
            streamWriter.WriteLine((char)12);
        }
        private string GetWTCode(DocumentStockReceiptList stockReceipt)
        {
            try
            {
                return stockReceipt.ItemList[0].WTCode;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetWTCode : " + ex.Message);
                return "0";
            }
        }
    }
   
}
