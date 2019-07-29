using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentDeliveryOrder
    {
        ManageReport manageReport = new ManageReport();
        public void GenerateDeliveryOrderText(DocumentDeliveryOrderEntity deliveryOrderList)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            bool isDuplicate = false;
            try
            {
                fileName = deliveryOrderList.Regioncode + GlobalVariable.DocumentDeliveryFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                manageReport.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + deliveryOrderList.UserID; //ManageReport.GetDateForFolder();
                manageReport.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                manageReport.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                isDuplicate = deliveryOrderList.UnLoadingSlip == "Y" ? true : false;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForDeliveryOrder(streamWriter, deliveryOrderList, isDuplicate);
                AddDetails(streamWriter, deliveryOrderList);
                AddFooter(streamWriter);
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
        /// <param name="deliveryOrderList"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForDeliveryOrder(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderEntity, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.WriteLine("|                                           ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(manageReport.StringFormat(deliveryOrderEntity.RegionName, 55, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            if (!isDuplicate)
            {
                streamWriter.WriteLine("|                                      DELIVERY ORDER            DUPLICATE COPY          |");
            }
            else
            {
                streamWriter.WriteLine("|                                      DELIVERY ORDER                                    |");
            }
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|TRANSACTION:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.TransactionName, 21, 2));
            streamWriter.Write("GODOWN NAME: ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.GodownName, 12, 2));
            streamWriter.Write("|DELIVERY ORDER: ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.Dono, 16, 2));
            streamWriter.Write("DATE: ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.DoDate.ToString(), 11, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|ISSUE TO:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.GodownName, 21, 2));
            streamWriter.WriteLine(" ");
            streamWriter.Write("|INDENT NO:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.IndentNo, 21, 2));
            // streamWriter.Write(manageReport.StringFormatWithoutPipe("DATE: ", 25, 1));
            streamWriter.Write("        PERMIT DATE:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(manageReport.FormatDate(deliveryOrderEntity.PermitDate.ToString()), 41, 2));
            streamWriter.Write("        ORDER PERIOD ENDING:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.OrderPeriod, 41, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("PAYMENT DETAILS");
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|-------------------------------------------------------------------------|");
            streamWriter.WriteLine("||-----------------------------------------------------------------------||");
            streamWriter.WriteLine("|| CH/DD NO          | DATE     | BANK NAME              | AMOUNT        |");
            streamWriter.WriteLine("||                   |          |                        |               ||");
            streamWriter.WriteLine("||----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("COMMODITY DETAILS");
            streamWriter.WriteLine("|| S.NO. | ITEM DESCRIPTION      | SCHEME              | QUANTITY TO BE ISSUED | RATE/UNIT   | TOTAL VALUE   |");
            streamWriter.WriteLine("||       |                       |                     |   KGS.(Net) GMS.      | RS.   P.    | RS.   P.     ||");
            streamWriter.WriteLine("||-------|-----------------------|---------------------|-----------------------| TOTAL       |-------------- |");
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("MARGIN RATE");
            streamWriter.WriteLine("|| S.NO. | ITEM DESCRIPTION      | SCHEME              | QUANTITY TO BE ISSUED | RATE/UNIT   | TOTAL VALUE   |");
            streamWriter.WriteLine("||       |                       |                     |   KGS.(Net) GMS.      | RS.   P.    | RS.   P.     ||");
            streamWriter.WriteLine("||-------|-----------------------|---------------------|-----------------------| TOTAL       |-------------- |");
            streamWriter.Write("|");
            
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddDetails(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderList)
        {
            int i = 0;
            foreach (var item in deliveryOrderList.deliveryPaymentDetails)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 4, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 11, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 14, 2));
                streamWriter.WriteLine(" ");
            }
            foreach (var item in deliveryOrderList.documentDeliveryItems)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 4, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 11, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 14, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 14, 2));
                streamWriter.WriteLine(" ");
            }
            foreach (var item in deliveryOrderList.deliveryMarginDetails)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 4, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 11, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 14, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 20, 2));
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 14, 2));
                streamWriter.WriteLine(" ");
            }
        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddFooter(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("||----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("| **(Pre.AmtBal+AmtPay) - (TotalValue+Others) = Balance >>> For Credit                                          |");
            streamWriter.Write("| **(Pre.AmtBal+TotalValue+Others) - (AmtPaid) = Balance >>> For Debit                                          |");
            streamWriter.WriteLine("|                                                                       SIGNATURE OF GODOWN INCHARGE        |");
            streamWriter.WriteLine("|REMARKS                                                                                                    |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("||----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);
        }

        }
    }
