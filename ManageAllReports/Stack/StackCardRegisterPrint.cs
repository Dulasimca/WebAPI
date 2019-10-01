using System;
using System.Collections.Generic;
using System.IO;
using TNCSCAPI.Controllers.Reports.Stack;

namespace TNCSCAPI.ManageAllReports.Stack
{
    public class StackCardRegisterPrint
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockIssuesEntity"></param>
        public void GenerateStackCardRegister(List<StackCardRegisterEntity> stackCardRegisters, StackEntity stackEntity)
        {
            // AuditLog.WriteError("GeneratestockIssuesEntityRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = stackEntity.GCode + GlobalVariable.StackCardRegister;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + stackEntity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);
                streamWriter = new StreamWriter(filePath, true);
                AddDocHeaderForIssues(streamWriter,stackEntity);
                AddDetails(streamWriter, stackCardRegisters);

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
        public void AddDocHeaderForIssues(StreamWriter streamWriter, StackEntity stackEntity)
        {
            streamWriter.WriteLine("                                                                           ");
            streamWriter.WriteLine("               TAMILNADU CIVIL SUPPLIES CORPORATION                        ");
            streamWriter.Write("                ");
            streamWriter.Write(report.StringFormatWithoutPipe("REGION : ", 9, 1));
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.RName, 45, 2));
            streamWriter.WriteLine("");
            streamWriter.Write("  Commodity: ");
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.ITName, 25, 2));
            streamWriter.Write("  Godown :");
            streamWriter.Write(report.StringFormatWithoutPipe(stackEntity.GName, 25, 2));
            streamWriter.WriteLine("");
            streamWriter.WriteLine("                                             Formation Year: " + stackEntity.StackDate);
            streamWriter.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            streamWriter.WriteLine("                                                   OPENING BALANCE RECEIPT         RECEIPT    ISSUE                   ISSUE   BALANCE            BALANCE  STACK");
            streamWriter.WriteLine("  SNO  STACKNO       FROM DATE  TO DATE            BAGS       QTY  BAGS     GU        QTY     BAGS           GR        QTY     BAGS                  QTY  STATUS   W/OFF QTY");
            streamWriter.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Add receipt item details
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="stockIssuesEntity"></param>
        private void AddDetails(StreamWriter streamWriter, List<StackCardRegisterEntity> stackCardRegisters)
        {
            try
            {
                int i = 1;
                string Fromdate = string.Empty;
                string Todate = string.Empty;
                int OpeningBag = 0;
                int ReceiptBag = 0;
                int IssuesBag = 0;
                int BalanceBag = 0;
                int GU = 0;
                int GR = 0;
                decimal OpeningQty = 0;
                decimal ReceiptQty = 0;
                decimal IssuesQty = 0;
                decimal BalanceQty = 0;
                decimal WriteOffQty = 0;
                foreach (var item in stackCardRegisters)
                {
                    streamWriter.Write(report.StringFormatWithoutPipe(i.ToString(), 4, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.StackCard, 13, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.FromDate, 10, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.ToDate, 10, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.OpeningBag, 9, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.OpeningQty, 11, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.ReceiptBag, 9, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.GU, 8, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.ReceiptQty, 11, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.IssuesBag, 9, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.GR, 8, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.IssuesQty, 11, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.BalanceBag, 9, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.BalanceQty, 15, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.StackStatus, 5, 1));
                    streamWriter.Write(report.StringFormatWithoutPipe(item.WriteOff, 9, 1));
                    streamWriter.WriteLine(" ");
                    streamWriter.WriteLine(" ");
                    OpeningBag += Convert.ToInt32(item.OpeningBag);
                    ReceiptBag += Convert.ToInt32(item.ReceiptBag);
                    IssuesBag += Convert.ToInt32(item.IssuesBag);
                    BalanceBag += Convert.ToInt32(item.BalanceBag);
                    GU += Convert.ToInt32(item.GU);
                    GR += Convert.ToInt32(item.GR);

                    OpeningQty += Convert.ToDecimal(item.OpeningQty);
                    ReceiptQty += Convert.ToDecimal(item.ReceiptQty);
                    IssuesQty += Convert.ToDecimal(item.IssuesQty);
                    BalanceQty += Convert.ToDecimal(item.BalanceQty);
                    WriteOffQty += Convert.ToDecimal(item.WriteOff);
                    i = i + 1;
                }
                streamWriter.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

                streamWriter.Write(report.StringFormatWithoutPipe(" ", 4, 1));
                streamWriter.Write(report.StringFormatWithoutPipe("-", 13, 1));
                streamWriter.Write(report.StringFormatWithoutPipe("-", 10, 1));
                streamWriter.Write(report.StringFormatWithoutPipe("-", 10, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(OpeningBag.ToString(), 9, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(OpeningQty.ToString(), 11, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(ReceiptBag.ToString(), 9, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(GU.ToString(), 8, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(ReceiptQty.ToString(), 11, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(IssuesBag.ToString(), 9, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(GR.ToString(), 8, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(IssuesQty.ToString(), 11, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(BalanceBag.ToString(), 9, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(BalanceQty.ToString(), 15, 1));
                streamWriter.Write(report.StringFormatWithoutPipe("-", 5, 1));
                streamWriter.Write(report.StringFormatWithoutPipe(WriteOffQty.ToString(), 9, 1));
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                streamWriter.WriteLine((char)12);
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
            }
   
        }

    }
}
