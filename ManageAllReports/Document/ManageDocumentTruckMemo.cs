using System;
using System.IO;
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
                fileName = truckMemoList.IssuingCode + GlobalVariable.DocumentTruckMemoFileName;
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
        /// <param name="truckMemoList"></param>
        /// <param name="isDuplicate"></param>
        public void AddDocHeaderForTruckMemo(StreamWriter streamWriter, DocumentStockTransferDetails truckMemoList, bool isDuplicate = false)
        {
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                       TAMILNADU CIVIL SUPPLIES CORPORATION                               |");
            streamWriter.Write("|                                           ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormat(truckMemoList.RegionName, 53, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                                                          |");
            if (isDuplicate)
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE TRUCK MEMO            DUPLICATE COPY                    |");
            }
            else
            {
                streamWriter.WriteLine("|                                      STOCK ISSUE TRUCK MEMO                                              |");
            }
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|TRUCK MEMO NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.STNo, 21, 2));
            streamWriter.Write("ALLOTMENT/RELEASE ORDER: ");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.RNo, 12, 2));
            streamWriter.Write("MOVE. ORDER NO: ");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.MNo, 15, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|              DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(truckMemoList.STDate.ToString()), 21, 2));
            streamWriter.Write(report.StringFormatWithoutPipe("TIME: ", 25, 1));
            streamWriter.Write(report.StringFormatWithoutPipe(report.GetCurrentTime(DateTime.Now), 16, 2));
            streamWriter.Write("DATE:");
            streamWriter.Write(report.StringFormatWithoutPipe(report.FormatDate(truckMemoList.RDate.ToString()), 16, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");

            streamWriter.Write("|ISSUING GODOWN   :");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.GodownName, 30, 2));
            streamWriter.Write("RECEIVER NAME: ");
            streamWriter.Write(report.StringFormatWithoutPipe(truckMemoList.ReceivingName, 41, 2));
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
        /// <param name="truckMemoList"></param>
        public void AddDetails(StreamWriter streamWriter, DocumentStockTransferDetails transferDetails)
        {
            int i = 0;
            foreach (var item in transferDetails.documentSTItemDetails)
            {
                i = i + 1;
                streamWriter.Write("||");
                streamWriter.Write(report.StringFormat(i.ToString(), 4, 2));
                streamWriter.Write(report.StringFormat(item.TStockNo, 11, 2));
                streamWriter.Write(report.StringFormat(item.ITDescription, 20, 2));
                streamWriter.Write(report.StringFormat(item.SchemeName, 14, 2));
                streamWriter.Write(report.StringFormat(item.PackingType, 13, 2));
                streamWriter.Write(report.StringFormat(item.NoPacking.ToString(), 6, 1));
                streamWriter.Write(report.StringFormat(item.GKgs.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(item.Nkgs.ToString(), 10, 1));
                streamWriter.Write(report.StringFormat(item.Moisture.ToString(), 8, 1) + "|");
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
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.Write("|LORRY NO: ");
            streamWriter.Write(report.StringFormatWithoutPipe(transferDetails.LorryNo, 23, 2));
            streamWriter.Write("TC NAME   : ");
            streamWriter.Write(report.StringFormatWithoutPipe(transferDetails.TransactionName, 59, 2) + "|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|MODE OF WEIGHMENT : ");
            streamWriter.Write(report.StringFormatWithoutPipe(GetWTCode(transferDetails), 85, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.Write("|FROM RAIL HEAD: ");
            streamWriter.Write(report.StringFormatWithoutPipe(transferDetails.RailHeadName, 17, 2));
            streamWriter.Write("WAGON NO  : ");
            streamWriter.Write(report.StringFormatWithoutPipe(GetWNo(transferDetails), 21, 2));
            streamWriter.Write("RR NO:");
            streamWriter.Write(report.StringFormatWithoutPipe(GetRRNo(transferDetails), 31, 2));
            streamWriter.Write("|");
            streamWriter.WriteLine(" ");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|The above stocks were weighed in our presence Received in Good Conditions and taken into account          |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|                                                                       NAME OF TC                         |");
            streamWriter.WriteLine("|                                                                                                          |");
            streamWriter.WriteLine("|GODOWN INCHARGE                                                        SIGNATURE OF THE TC REPRESENTATIVE |");
            streamWriter.WriteLine("|REMARKS                                                                                                   |");
            streamWriter.WriteLine("|   " + report.StringFormatWithoutPipe(GetRemarks(transferDetails), 103, 2) + "|");
            streamWriter.WriteLine("|----------------------------------------------------------------------------------------------------------|");
            streamWriter.WriteLine((char)12);
        }

        public string GetWTCode(DocumentStockTransferDetails transferDetails)
        {
            string WtCode = string.Empty;
            try
            {
                WtCode = transferDetails.documentSTItemDetails[0].WTCode;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" GetWTCode " + ex.Message + " " + ex.StackTrace);
            }
            return WtCode;
        }

        public string GetRemarks(DocumentStockTransferDetails transferDetails)
        {
            try
            {
                return transferDetails.documentSTTDetails[0].Remarks;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" GetRemarks " + ex.Message + " " + ex.StackTrace);
            }
            return "-";
        }

        public string GetWNo(DocumentStockTransferDetails transferDetails)
        {
            string WtCode = string.Empty;
            try
            {
                WtCode = transferDetails.documentSTTDetails[0].Wno;
            }
            catch
            {
            }
            return WtCode;
        }


        public string GetRRNo(DocumentStockTransferDetails transferDetails)
        {
            string WtCode = string.Empty;
            try
            {
                WtCode = transferDetails.documentSTTDetails[0].RRNo;
            }
            catch
            {
            }
            return WtCode;
        }
    }
}

