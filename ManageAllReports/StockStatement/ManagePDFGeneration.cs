using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TNCSCAPI.Models.Documents;

namespace TNCSCAPI.ManageAllReports.StockStatement
{
    public class ManagePDFGeneration
    {
        ManageReport report = new ManageReport();
        Font NormalFont = FontFactory.GetFont("Courier New", 8, Font.NORMAL, BaseColor.Black);
        Font FSSAIFont = FontFactory.GetFont("Courier New", 10, Font.NORMAL, BaseColor.Black);
        public Tuple<bool, string> GeneratePDF(DocumentStockReceiptList stockReceipt = null)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            try
            {
                fileName = stockReceipt.ReceivingCode + GlobalVariable.ReceiptAckFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + stockReceipt.UserID; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".pdf";
                report.DeleteFileIfExists(filePath);

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 5f, 5f, 5f, 5f);
                //Create PDF Table  
                FileStream fs = new FileStream(filePath, FileMode.Create);
                //Create a PDF file in specific path  
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                // Add meta information to the document  
                document.AddAuthor("Dulasiayya from BonTon");
                document.AddCreator("Acknolowdgement for particuar document");
                document.AddKeywords("TNCSC- Webcopy ");
                document.AddSubject("Document subject - Ack Web Copy ");
                document.AddTitle("The document title - PDF creation for Receipt Document");

                //Open the PDF document  
                document.Open();
                string imagePath = GlobalVariable.ReportPath + "layout\\images\\dashboard\\tncsc-logo.PNG";
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                img.Alignment = Element.ALIGN_CENTER;
                img.ScaleToFit(80f, 60f);

                imagePath = GlobalVariable.ReportPath + "layout\\images\\dashboard\\watermark.PNG";
                iTextSharp.text.Image imgWaterMark = iTextSharp.text.Image.GetInstance(imagePath);
                imgWaterMark.ScaleToFit(300, 450);
                imgWaterMark.Alignment = iTextSharp.text.Image.UNDERLYING;
                imgWaterMark.SetAbsolutePosition(120, 450);
                document.Add(imgWaterMark);
                //|----------------------------------------------------------------------------------------------------------|
                //Create the table 
                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //table.setBorder(Border.NO_BORDER);
                //set overall width
                table.WidthPercentage = 100f;
                //set column widths
                int[] firstTablecellwidth = { 20, 80 };
                table.SetWidths(firstTablecellwidth);
                //iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", "16", iTextSharp.text.Font.NORMAL);
                PdfPCell cell = new PdfPCell(img);
                cell.Rowspan = 3;
                cell.BorderWidth = 0;
                // cell.Border = (Border.NO_BORDER);
                table.AddCell(cell);
                PdfPCell cell1 = new PdfPCell(new Phrase("TAMILNADU CIVIL SUPPLIES CORPORATION"));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.BorderWidth = 0;
                table.AddCell(cell1);


                cell1 = new PdfPCell(new Phrase("Region Name : " + stockReceipt.RegionName));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.BorderWidth = 0;
                table.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.BorderWidth = 0;
                table.AddCell(cell1);

                document.Add(table);
                Paragraph heading = new Paragraph("           STOCK RECEIPT ACKNOWLEDGMENT         WEB COPY");
                heading.Alignment = Element.ALIGN_CENTER;
                document.Add(heading);
                AddSpace(document);
                AddHRLine(document);
                //add header values
                AddheaderValues(document, stockReceipt);
                AddSpace(document);
                AddDetails(document, stockReceipt);
                AddSpace(document);
                AddLorryInfo(document, stockReceipt);
                AddSpace(document);
                AddFSSAI(document);
                AddSign(document);
                AddSpace(document);
                AddRemarks(document, stockReceipt);
                AddSpace(document);
                AddHRLine(document);
                AddFooter(document, stockReceipt);
                //Add border to page
                PdfContentByte content = writer.DirectContent;
                Rectangle rectangle = new Rectangle(document.PageSize);
                rectangle.Left += document.LeftMargin;
                rectangle.Right -= document.RightMargin;
                rectangle.Top -= document.TopMargin;
                rectangle.Bottom += document.BottomMargin;
                content.SetColorStroke(BaseColor.Black);
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();
                // Close the document  
                document.Close();
                // Close the writer instance  
                writer.Close();
                // Always close open filehandles explicity  
                fs.Close();

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" GeneratePDF :  " + ex.Message + " " + ex.StackTrace);
                return new Tuple<bool, string>(false, "Please Contact system Admin");
            }
            return new Tuple<bool, string>(true, "Print Generated Successfully");
        }
        public void AddHRLine(iTextSharp.text.Document doc)
        {
            LineSeparator line = new LineSeparator(1f, 100f, BaseColor.Black, Element.ALIGN_LEFT, 1);
            doc.Add(line);
        }

        public void AddSpace(iTextSharp.text.Document doc)
        {
            Paragraph heading = new Paragraph("");
            heading.Alignment = Element.ALIGN_CENTER;
            heading.SpacingAfter = 7f;
            doc.Add(heading);
        }

        public void AddheaderValues(iTextSharp.text.Document doc, DocumentStockReceiptList stockReceipt)
        {
            PdfPTable table = new PdfPTable(6);
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //table.setBorder(Border.NO_BORDER);
            //set overall width
            table.WidthPercentage = 100f;
            //set column widths
            int[] firstTablecellwidth = { 20, 20, 20, 20, 10, 10 };
            table.SetWidths(firstTablecellwidth);

            PdfPCell cell = new PdfPCell(new Phrase("ACKNOWLEDGEMENT NO:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(stockReceipt.SRNo, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("ALLOTMENT/RELEASE ORDER:" + stockReceipt.OrderNo + " " + stockReceipt.PAllotment, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Colspan = 4;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DATE:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(report.FormatDate(stockReceipt.SRDate.ToString()), NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DATE:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(report.FormatDate(stockReceipt.OrderDate.ToString()), NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("GATE PASS:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("PERIOD OF ALLOTMENT:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(report.FormatDate(stockReceipt.SRDate.ToString()), NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Transaction Type:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(stockReceipt.TransactionName, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 3;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("RECEIVING GODOWN:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(stockReceipt.GodownName, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DEPOSITOR'S NAME:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(stockReceipt.DepositorName, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 3;
            table.AddCell(cell);

            doc.Add(table);

        }

        public void AddDetails(iTextSharp.text.Document doc, DocumentStockReceiptList stockReceipt)
        {
            //streamWriter.WriteLine("||SNo |STACK NO   |COMMODITY           | SCHEME       |UNIT WEIGHT  |NO.OF UNIT |  Gross WEIGHT        NET   |% OF MOISTURE   ||");
            //streamWriter.WriteLine("||    |           |                    |              |             |  UNIT|   WEIGHT in Kgs/NOs |MOISTURE||");

            PdfPTable table = new PdfPTable(9);
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //set overall width
            table.WidthPercentage = 100f;
            //set column widths
            int[] firstTablecellwidth = { 5, 8, 20, 12, 18, 7, 10, 10, 10 };
            table.SetWidths(firstTablecellwidth);

            PdfPCell cell = new PdfPCell(new Phrase("SNo", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("STACK NO", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("COMMODITY", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("SCHEME", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("UNIT WEIGHT", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("NO.OF UNIT", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Gross WEIGHT", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("NET WEIGHT", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("% OF MOISTURE", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);



            int i = 0;
            foreach (var item in stockReceipt.ItemList)
            {
                i++;
                cell = new PdfPCell(new Phrase(i.ToString(), NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.TStockNo, NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.CommodityName, NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.SchemeName, NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.PackingName, NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.NoPacking.ToString(), NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(report.DecimalformatForWeight(item.GKgs.ToString()), NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(report.DecimalformatForWeight(item.Nkgs.ToString()), NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.Moisture.ToString(), NormalFont));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
            }
            doc.Add(table);
        }

        public void AddLorryInfo(iTextSharp.text.Document doc, DocumentStockReceiptList stockReceipt)
        {
            PdfPTable table = new PdfPTable(6);
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //table.setBorder(Border.NO_BORDER);
            //set overall width
            table.WidthPercentage = 100f;
            table.DefaultCell.Padding = 10;
            //set column widths
            int[] firstTablecellwidth = { 12, 18, 15, 25, 12, 18 };
            table.SetWidths(firstTablecellwidth);

            PdfPCell cell = new PdfPCell(new Phrase("T.MEMO/IN.NO:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(stockReceipt.TruckMemoNo, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("LORRY NO       : " + stockReceipt.LNo, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Colspan = 2;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("TC NAME       : " + stockReceipt.TransporterName, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("T.MEMO/IN.DT:", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(report.FormatDate(stockReceipt.TruckMemoDate.ToString()), NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("LORRY FROM    : " + stockReceipt.LFrom, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("MODE OF WEIGHMENT : " + GetWTCode(stockReceipt), NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("WAGON NO       : -", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Colspan = 2;
            cell.BorderWidth = 0;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("RR NO         : " + stockReceipt.MTransport, NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            cell.Colspan = 4;
            table.AddCell(cell);
            doc.Add(table);

        }

        public void AddFSSAI(iTextSharp.text.Document doc)
        {
            Paragraph FSSAI = new Paragraph("     " + GlobalVariable.FSSAI1, FSSAIFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);
            FSSAI = new Paragraph("     " + GlobalVariable.FSSAI2, FSSAIFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);
            FSSAI = new Paragraph("     " + GlobalVariable.FSSAI3, FSSAIFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);

            AddSpace(doc);

        }

        public void AddSign(iTextSharp.text.Document doc,string GCode)
        {
            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //table.setBorder(Border.NO_BORDER);
            //set overall width
            table.WidthPercentage = 100f;
            table.DefaultCell.Padding = 10;
            //set column widths
            int[] firstTablecellwidth = { 60, 40 };
            table.SetWidths(firstTablecellwidth);

            PdfPCell cell = new PdfPCell(new Phrase("Sign. of the Authorised Person.", FSSAIFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("GODOWN INCHARGE ", FSSAIFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", NormalFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            //Get the file name
            var result = GetImageName(GCode);
            if (result.Item1)
            {
                string imagePath = GlobalVariable.ReportPath + "layout\\images\\InchargeSignature\\" + result.Item2;
                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.Alignment = Element.ALIGN_CENTER;
                    img.ScaleToFit(200f, 80f);
                    cell = new PdfPCell(img);
                }
                else
                {
                    cell = new PdfPCell(new Phrase("", NormalFont));
                }

            }
            else
            {
                cell = new PdfPCell(new Phrase("", NormalFont));
            }
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidth = 0;
            cell.MinimumHeight = 80f;
            table.AddCell(cell);
            doc.Add(table);

        }

        public void AddRemarks(iTextSharp.text.Document doc, DocumentStockReceiptList stockReceipt)
        {
            Paragraph FSSAI = new Paragraph(" REMARKS ", FSSAIFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);
            FSSAI = new Paragraph("     " + stockReceipt.Remarks, NormalFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);
            AddSpace(doc);

        }
        public void AddFooter(iTextSharp.text.Document doc, DocumentStockReceiptList stockReceipt)
        {
            //GetDate()
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            DateTime dateTime = manageSQL.GetSRTime(stockReceipt.SRNo);
            string receiptDateTime = report.FormatDate(stockReceipt.SRDate.ToString()) + " " + report.GetCurrentTime(dateTime);
            Paragraph FSSAI = new Paragraph("  Prepared DateTime:" + receiptDateTime + "      Printing DateTime:" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") + "   User : " + stockReceipt.UserID, NormalFont);
            FSSAI.Alignment = Element.ALIGN_LEFT;
            doc.Add(FSSAI);
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

        private Tuple<bool, string> GetImageName(string GCode)
        {
            try
            {
                DataSet ds = new DataSet();
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@GodownCode", GCode));
                ds = manageSQLConnection.GetDataSetValues("GetGodownProfile", sqlParameters);
                ManageReport manageReport = new ManageReport();
                if (manageReport.CheckDataAvailable(ds))
                {
                    return new Tuple<bool, string>(true, Convert.ToString(ds.Tables[0].Rows[0]["ImageName"]));
                }
                return new Tuple<bool, string>(false,"" );

            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetImageName : " + ex.Message);
                return new Tuple<bool, string>(false, "");

            }
        }
    }
}
