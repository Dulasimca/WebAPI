using System;
using System.IO;
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
            try
            {
                fileName = deliveryOrderList.IssuerCode + GlobalVariable.DocumentDOFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                manageReport.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + deliveryOrderList.UserID; //ManageReport.GetDateForFolder();
                manageReport.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                manageReport.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForDeliveryOrder(streamWriter, deliveryOrderList);
                AddPaymentDetails(streamWriter, deliveryOrderList);
                AddItemsDetails(streamWriter, deliveryOrderList);
                AddMarginDetails(streamWriter, deliveryOrderList);
                AddFooter(streamWriter, deliveryOrderList);


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
        /// <param name="deliveryOrderList"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForDeliveryOrder(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderEntity, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|==========================================================================================================|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.Write("|                                              ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(manageReport.StringFormat(deliveryOrderEntity.RegionName, 50, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                      DELIVERY ORDER                                                      |");
            streamWriter.WriteLine("|==========================================================================================================|");
            streamWriter.Write("|TRANSACTION:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.TransactionName, 21, 2));
            streamWriter.Write("GODOWN NAME: ");
            streamWriter.Write(manageReport.StringFormat(deliveryOrderEntity.GodownName, 59, 2));
            streamWriter.WriteLine(" ");
            streamWriter.Write("|DELIVERY ORDER: ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.Dono, 16, 2));
            streamWriter.Write("DATE: ");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(manageReport.FormatIndianDate(deliveryOrderEntity.DoDate.ToString()), 66, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|==========================================================================================================|");

            streamWriter.Write("|ISSUE TO:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.ReceivorName, 96, 2) + "|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|INDENT NO:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.IndentNo, 21, 2));
            // streamWriter.Write(manageReport.StringFormatWithoutPipe("DATE: ", 25, 1));
            streamWriter.Write("PERMIT DATE:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(manageReport.FormatDate(deliveryOrderEntity.PermitDate.ToString()), 13, 2));
            streamWriter.Write("ORDER PERIOD ENDING:");
            streamWriter.Write(manageReport.StringFormatWithoutPipe(deliveryOrderEntity.OrderPeriod, 27, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|==========================================================================================================|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddPaymentDetails(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderList)
        {
            double dTotal = 0, dTotalADJ = 0;

            int i = 0;
            foreach (var item in deliveryOrderList.deliveryPaymentDetails)
            {
                if (i == 0)
                {
                    streamWriter.WriteLine("|PAYMENT DETAILS                                                                                           |");
                    streamWriter.WriteLine("|                                                                                                          |");
                    streamWriter.WriteLine("||------------------------------------------------------|                                                  |");
                    streamWriter.WriteLine("||CHQ/DD NO |  DATE    |     BANK NAME      |  AMOUNT   |                                                  |");
                    streamWriter.WriteLine("||------------------------------------------------------|                                                  |");
                }
                i = 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(item.ChequeNo, 10, 2));
                streamWriter.Write(manageReport.StringFormat(manageReport.FormatIndianDate(item.ChDate), 10, 2));
                streamWriter.Write(manageReport.StringFormat(item.bank, 20, 2));
                streamWriter.Write(manageReport.StringFormat(item.PaymentAmount.ToString(), 11, 2));
                streamWriter.Write("                                                  |");
                streamWriter.WriteLine(" ");
                dTotal = dTotal + Convert.ToDouble(item.PaymentAmount);
            }
            if (i == 1)
            {
                streamWriter.WriteLine("||------------------------------------------------------|                                                  |");
                streamWriter.Write("||                                  TOTAL   |");
                streamWriter.Write(manageReport.StringFormat(dTotal.ToString(), 11, 2));
                streamWriter.Write("                                                  |");
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("||------------------------------------------------------|                                                  |");

            }
            streamWriter.WriteLine("|                                                                                                          |");
            int j = 0;
            foreach (var item in deliveryOrderList.deliveryAdjustmentDetails)
            {
                if (j == 0)
                {
                    streamWriter.WriteLine("|ADJUSTMENT                                                                                                |");
                    streamWriter.WriteLine("|                                                                                                          |");
                    streamWriter.WriteLine("||--------------------------------------------------------------------------------------|                  |");
                    streamWriter.WriteLine("|| PREVIOUS  | PREVIOUS  | BY ADJUSTMENT | Pre.AMT.BAL | OTHER.AMT.    |  BALANCE**     |                  |");
                    streamWriter.WriteLine("|| D.O.BO.   | D.O.DT.   |               |    Cr./Dr.  |    ADJ.       |                |                  |");
                    streamWriter.WriteLine("||--------------------------------------------------------------------------------------|                  |");
                }
                j = 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(item.AdjustedDoNo, 11, 2));
                streamWriter.Write(manageReport.StringFormat(manageReport.FormatIndianDate(item.AdjustDate), 11, 2));
                streamWriter.Write(manageReport.StringFormat(item.AdjustmentType, 13, 2));
                streamWriter.Write(manageReport.StringFormat(manageReport.Decimalformat(item.Amount.ToString()), 15, 1));
                streamWriter.Write(manageReport.StringFormat(manageReport.Decimalformat(item.AmountNowAdjusted.ToString()), 15, 1));
                streamWriter.Write(manageReport.StringFormat(manageReport.Decimalformat(item.Balance.ToString()), 16, 1));
                streamWriter.Write("                  |");
                streamWriter.WriteLine(" ");
                dTotalADJ = dTotalADJ + Convert.ToDouble(item.Balance);
            }
            if (j == 1)
            {
                streamWriter.WriteLine("||--------------------------------------------------------------------------------------|                  |");
                streamWriter.Write("||                                                           TOTAL     |");
                streamWriter.Write(manageReport.StringFormat(manageReport.Decimalformat(dTotalADJ.ToString()), 16, 1));
                streamWriter.Write("                  |");
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("||--------------------------------------------------------------------------------------|                  |");
            }
            streamWriter.WriteLine("|==========================================================================================================|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddItemsDetails(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderList)
        {
            int i = 0;
            double dTotal = 0;
            streamWriter.WriteLine("| Commodity Details                                                                                        |");
            streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");
            streamWriter.WriteLine("||S.NO. |   ITEM DESCRIPTION      |SCHEME             |QUANTITY TO BE ISSUED|RATE/UNIT  | TOTAL VALUE |    |");
            streamWriter.WriteLine("||      |                         |                   |    KGS.(Nett) GMS.  | RS.   P.  |   RS.    P. |    |");
            streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");

            foreach (var item in deliveryOrderList.documentDeliveryItems)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 6, 2));
                streamWriter.Write(manageReport.StringFormat(item.ITDescription, 25, 2));
                streamWriter.Write(manageReport.StringFormat(item.SchemeName, 19, 2));
                streamWriter.Write(manageReport.StringFormat(item.NetWeight.ToString(), 21, 2));
                streamWriter.Write(manageReport.StringFormat(item.Rate.ToString(), 11, 1));
                streamWriter.Write(manageReport.StringFormat(item.Total.ToString(), 13, 1));
                streamWriter.Write("    |");
                streamWriter.WriteLine(" ");
                dTotal = dTotal + Convert.ToDouble(item.Total);
            }
            streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");
            streamWriter.Write("|                                                                            TOTAL      |");
            streamWriter.Write(manageReport.StringFormat(dTotal.ToString(), 13, 1));
            streamWriter.Write("    |");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|==========================================================================================================|");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddMarginDetails(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderList)
        {
            int i = 0;
            double dTotal = 0;
            foreach (var item in deliveryOrderList.deliveryMarginDetails)
            {
                if (i == 0)
                {
                    streamWriter.WriteLine("|  Margin Rate                                                                                             |");
                    streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");
                    streamWriter.WriteLine("||S.NO. |   ITEM DESCRIPTION      |SCHEME             |QUANTITY TO BE ISSUED|RATE/UNIT  | TOTAL VALUE |    |");
                    streamWriter.WriteLine("||      |                         |                   |    KGS.(Nett) GMS.  | RS.   P.  |   RS.    P. |    |");
                    streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");
                }
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(manageReport.StringFormat(i.ToString(), 6, 2));
                streamWriter.Write(manageReport.StringFormat(item.ITDescription, 25, 2));
                streamWriter.Write(manageReport.StringFormat(item.SchemeName, 19, 2));
                streamWriter.Write(manageReport.StringFormat(item.MarginNkgs.ToString(), 21, 2));
                streamWriter.Write(manageReport.StringFormat(item.MarginRate.ToString(), 11, 1));
                streamWriter.Write(manageReport.StringFormat(item.MarginAmount.ToString(), 13, 1));
                streamWriter.Write("    |");
                streamWriter.WriteLine(" ");
                dTotal = dTotal + Convert.ToDouble(item.MarginAmount);
            }
            if (i > 0)
            {
                streamWriter.WriteLine("||------|-------------------------|-------------------|---------------------|-----------|-------------|    |");
                streamWriter.Write("|                                                                            TOTAL      |");
                streamWriter.Write(manageReport.StringFormat(dTotal.ToString(), 13, 1));
                streamWriter.Write("    |");
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("|==========================================================================================================|");
            }
        }
        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockReceipt"></param>
        public void AddFooter(StreamWriter streamWriter, DocumentDeliveryOrderEntity deliveryOrderList)
        {
            ManageReport report = new ManageReport();
            streamWriter.WriteLine("|**(Pre.AmtBal+AmtPay)-(TotalValue+Others)  = Balance >>> For Credit                                       |");
            streamWriter.WriteLine("|**(Pre.AmtBal+TotalValue+others)-(Amtpaid) = Balance >>> For Debit                                        |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                    SIGNATURE OF GODOWN INCHARGE          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|REMARKS                                                                                                   |");
            streamWriter.WriteLine("|   " + manageReport.StringFormatWithoutPipe(deliveryOrderList.Remarks, 102, 2) + "|");
            report.AddMoreContent(streamWriter, deliveryOrderList.Remarks, 102, 3);//Add content next line.
            streamWriter.WriteLine("|==========================================================================================================|");
            streamWriter.WriteLine((char)12);
        }

    }
}
