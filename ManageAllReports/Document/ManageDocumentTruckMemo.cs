using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentTruckMemo
    {
        ManageReport report = new ManageReport();
        public void GenerateTruckMemo(DocumentStockTransferDetails truckMemoList)
        {
            AuditLog.WriteError("GenerateTruckMemoRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            bool isDuplicate = false;
            try
            {
                fileName = truckMemoList.ReceivingCode + GlobalVariable.DocumentReceiptFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + truckMemoList.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                //  isDuplicate = ReceiptId == "0" ? false : true;
                isDuplicate = truckMemoList.IssueSlip == "Y" ? true : false;
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForTruckMemo(streamWriter, truckMemoList, isDuplicate);
                AddDetails(streamWriter, truckMemoList);
                AddFooter(streamWriter, truckMemoList);
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
        /// <param name="truckMemoList"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForTruckMemo(StreamWriter streamWriter, DocumentStockTransferDetails truckMemoList, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.WriteLine("|                                           ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(truckMemoList.RegionName, 55, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            if (!isDuplicate)
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE TRUCK MEMO            DUPLICATE COPY          |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE TRUCK MEMO                                    |");
            }
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|TRUCK MEMO NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.STNo, 21, 2));
            streamWriter.Write("ALLOTMENT/RELEASE ORDER: ");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.MNo, 12, 2));
            streamWriter.Write("MOVE. ORDER NO: ");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(truckMemoList.MDate.ToString()), 16, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|              DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(truckMemoList.RDate.ToString()), 21, 2));
            streamWriter.Write(report.StringFormatWithoutPipe("TIME: ", 25, 1));
            streamWriter.Write(report.StringFormatWithoutPipe("", 41, 2));
            streamWriter.Write("DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(truckMemoList.RDate.ToString()), 21, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|ISSUING GODOWN   :");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.GodownName, 30, 2));
            streamWriter.Write("RECEIVER NAME: ");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.ReceivingName, 41, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.WriteLine("|-----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("||---------------------------------------------------------------------------------------------------------||");
            streamWriter.WriteLine("||SNo |STACK NO   |COMMODITY           | SCHEME       |UNIT WEIGHT  |NO.OF |  Gross        NET   |% OF     ||");
            streamWriter.WriteLine("||    |           |                    |              |             |  UNIT|   WEIGHT in Kgs/NOs |MOISTURE ||");
            streamWriter.WriteLine("||---------------------------------------------------------------------------------------------------------||");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="truckMemoList"></param>
        public void AddDetails(StreamWriter streamWriter, DocumentStockTransferDetails transferDetails)
        {
            int i = 0;
            foreach (var item in transferDetails.documentSTItemDetails)
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
                streamWriter.Write(report.StringFormat(i.ToString(), 8, 1));
                streamWriter.WriteLine(" ");
            }
        }

        /// <summary>
        /// Add footer for document receipt
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="truckMemoList"></param>
        public void AddFooter(StreamWriter streamWriter, DocumentStockTransferDetails transferDetails)
        {
            streamWriter.WriteLine("|-----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|LORRY NO: ");
            streamWriter.Write(report.StringFormatWithoutPipe(transferDetails.LorryNo, 13, 2));
            streamWriter.Write("TC NAME      : ");
            streamWriter.Write(report.StringFormatWithoutPipe(transferDetails.TransactionName, 14, 2));
            streamWriter.Write("|MODE OF WEIGHMENT : ");
            streamWriter.Write(report.StringFormatWithoutPipe("", 28, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|FROM RAIL HEAD: ");
            streamWriter.Write(report.StringFormatWithoutPipe("-", 13, 2));
            streamWriter.Write("WAGON NO : ");
            streamWriter.Write(report.StringFormatWithoutPipe("-", 59, 2));
            streamWriter.Write("RR NO:");
            streamWriter.WriteLine(report.StringFormatWithoutPipe("-", 59, 2));
            streamWriter.Write("|");
            streamWriter.Write(" ");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|The above stocks were weighed in our presence Received in Good Conditions and taken into account           |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|                                                                       NAME OF TC                          |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|GODOWN INCHARGE                                                        SIGNATURE OF THE TC REPRESENTATIVE  |");
            streamWriter.WriteLine("|REMARKS                                                                                                    |");
            streamWriter.WriteLine("|                                                                                                           |");
            streamWriter.WriteLine("|-----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);   
        }
    }
}

