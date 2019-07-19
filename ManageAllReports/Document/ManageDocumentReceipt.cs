using System;
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
            AuditLog.WriteError("GenerateStockReceiptRegister");
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
                isDuplicate = stockReceipt.UnLoadingSlip == "Y" ? true : false;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForReceipt(streamWriter, stockReceipt, isDuplicate);
                AddDetails(streamWriter, stockReceipt);
                AddFooter(streamWriter, stockReceipt);
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
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
            if(!isDuplicate)
            {
                streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT            DUPLICATE COPY          |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT                                    |");
            }
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|ACKNOWLEDGEMENT NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.SRNo, 21, 2));
            streamWriter.Write("ALLOTMENT/RELEASE ORDER: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.PAllotment, 12, 2));
            streamWriter.Write("GATE PASS : ");
            streamWriter.Write(report.StringFormatWithoutPipe("", 14, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|              DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.SRDate.ToString()), 21, 2));
            streamWriter.Write(report.StringFormatWithoutPipe("DATE: ",25,1));
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.OrderDate.ToString()), 39, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|PERIOD OF ALLOTMENT:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.SRDate.ToString()), 30, 2));
            streamWriter.Write("Transaction Type: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.TransactionType, 36, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|RECEIVING GODOWN   :");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.GodownName, 30, 2));
            streamWriter.Write("DEPOSITOR'S NAME: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.DepositorName, 36, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("||--------------------------------------------------------------------------------------------------------||");
            streamWriter.WriteLine("||SNo |STACK NO   |COMMODITY           | SCHEME       |UNIT WEIGHT  |NO.OF |  Gross        NET   |% OF     ||");
            streamWriter.WriteLine("||    |           |                    |              |             |  UNIT|   WEIGHT in Kgs/NOs |MOISTURE ||");
            streamWriter.WriteLine("||---------------------------------------------------------------------------------------------------------||");
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
                streamWriter.Write(report.StringFormat(i.ToString(), 11, 2));
                streamWriter.Write(report.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(report.StringFormat(i.ToString(), 14, 2));
                streamWriter.Write(report.StringFormat(i.ToString(), 13, 2));
                streamWriter.Write(report.StringFormat(i.ToString(), 6, 1));
                streamWriter.Write(report.StringFormat(i.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(i.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(i.ToString(), 8, 1) +"|");
                streamWriter.WriteLine(" ");
            }
            streamWriter.WriteLine((char)12);
        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddFooter(StreamWriter streamWriter, DocumentStockReceiptList stockReceipt)
        {
            streamWriter.WriteLine("|-----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|T.MEMO/INVOICE NO: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.TruckMemoNo, 13, 2));
            streamWriter.Write("LORRY NO      : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.LNo, 14, 2));
            streamWriter.Write("TC NAME       : ");
            streamWriter.Write(report.StringFormatWithoutPipe(" ", 25, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|T.MEMO/INVOICE DT: ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(stockReceipt.TruckMemoDate.ToString()), 13, 2));
            streamWriter.Write("LORRY FROM    : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.LFrom, 56, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.Write("|MODE OF WEIGHMENT: ");
            streamWriter.Write(report.StringFormatWithoutPipe(" ", 13, 2));
            streamWriter.Write("WAGON NO      : ");
            streamWriter.Write(report.StringFormatWithoutPipe(" ", 14, 2));
            streamWriter.Write("RR NO         : ");
            streamWriter.Write(report.StringFormatWithoutPipe(stockReceipt.MTransport, 25, 2));
            streamWriter.Write("|");
            streamWriter.Write(" ");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|The above stocks were weighed in our presence Received in Good Conditions and taken into account           |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|DEPOSITOR OR HIS REPRESENTATIVE                                               GODOWN INCHARGE              |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|REMARKS                                                                                                    |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|-----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine(" Prepared DateTime:"+ stockReceipt.SRDate+ "             Printing DateTime:"+DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));
        }
    }
}
