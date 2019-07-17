using System;
using System.Collections.Generic;
using System.IO;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentReceipt
    {
        ManageReport report = new ManageReport();
        public void GenerateReceipt(DocumentStockReceiptList stockReceipt, string ReceiptId)
        {
            AuditLog.WriteError("GenerateStockReceiptRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
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

                streamWriter = new StreamWriter(filePath, true);
                //  DateWiseStockReceiptRegister(streamWriter, entity);

                List<StockReceiptList> stockReceiptList = new List<StockReceiptList>();
                // stockReceiptList = report.ConvertDataTableToList<StockReceiptList>(entity.dataSet.Tables[0]);

                // DateWiseStockReceiptRegister(streamWriter, entity);
                //StockReceiptAbstractRecdTypeAndSchemeWise(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractSchemeAndCommodityWise(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractStackNoAndCommodity(streamWriter, stockReceiptList, entity);
                //StockReceiptAbstractCommodityWise(streamWriter, stockReceiptList, entity);

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

        public void AddDocHeaderForReceipt(StreamWriter streamWriter, DocumentStockReceiptList stockReceipt, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.WriteLine("|                                           ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(stockReceipt.GodownName, 55, 2));
            streamWriter.WriteLine("");
            //streamWriter.WriteLine("|                                           REGION : DINDIGUL                                              |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            if(!isDuplicate)
            {
                streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT            DUPLICATE COPY              |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK RECEIPT ACKNOWLEDGMENT                                        |");
            }
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("          Stock Receipt Register:" + report.FormatDate(date) + "           Godown : " + GName + "          Region :" + RName);
            streamWriter.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("S.No|  Ack No   |Truck Memo No      | Lorry No  |   From Whom Received            |   Scheme   |  Stack No  |No bags |   Commodity   |Net Weight|");
            streamWriter.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("    |           |                   |           |                                 |            |            |        |               |          |");

        }
    }
}
