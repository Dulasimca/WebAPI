using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.Document
{
    public class ManageDocumentReceipt
    {
        public void GenerateReceipt(DocumentStockReceiptList stockReceipt,string ReceiptId)
        {
            AuditLog.WriteError("GenerateStockReceiptRegister");
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {               
                fileName = stockReceipt.ReceivingCode + GlobalVariable.StockReceiptRegisterFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                DateWiseStockReceiptRegister(streamWriter, entity);

                List<StockReceiptList> stockReceiptList = new List<StockReceiptList>();
                stockReceiptList = report.ConvertDataTableToList<StockReceiptList>(entity.dataSet.Tables[0]);

                // DateWiseStockReceiptRegister(streamWriter, entity);
                StockReceiptAbstractRecdTypeAndSchemeWise(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractSchemeAndCommodityWise(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractStackNoAndCommodity(streamWriter, stockReceiptList, entity);
                StockReceiptAbstractCommodityWise(streamWriter, stockReceiptList, entity);

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

        public void 
    }
}
