using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TNCSCAPI.Models;

namespace TNCSCAPI.Mail
{
    public class SendMail
    {
        public bool MailSending(MailEntity mailEntity)
        {
            bool isSend = false;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(mailEntity.FromMailid);
                //Add mutiple too
                string[] ToMuliId = mailEntity.ToMailid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ToEMailId in ToMuliId)
                {
                    message.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                string[] ToMuliCC = mailEntity.ToCC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string CCEMailId in ToMuliCC)
                {
                    message.CC.Add(new MailAddress(CCEMailId)); //adding multiple CC Email Id
                }
                //Add multiple CC
                message.Subject = mailEntity.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailEntity.BodyMessage;
                smtp.Port = mailEntity.Port;
                smtp.Host = mailEntity.SMTP; //for gmail host  
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailEntity.FromMailid, mailEntity.FromPassword);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                isSend = true;
            }
            catch (Exception ex)
            {
                throw ex;
                //write error
            }
            return isSend;
        }
        public string BodyMessage(BodyMessageEntity bodyMessageEntity)
        {
            try
            {
                string messageBody = "<font>Hi Team, </font><br><br>";
                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style=\"color:#555555;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                string htmlTdEnd = "</td>";
                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += "Please provide the quotation for below mentioned products.";
                messageBody += htmlTdStart + "Email id" + bodyMessageEntity.Mailid;
                messageBody += htmlTdStart + "Phone Number" + bodyMessageEntity.PhoneNumber;
                messageBody += htmlTdStart + "Products" + bodyMessageEntity.Products;
                messageBody += htmlTdStart + "Remarks" + bodyMessageEntity.Remarks;
                messageBody += htmlHeaderRowEnd;
                //Loop all the rows from grid vew and added to html td  
                messageBody = messageBody + htmlTrStart + htmlTdStart;
                messageBody = messageBody + "Thanks & Regads" + htmlTdStart;
                messageBody = messageBody + htmlTdStart + " SI Support Team" + htmlTdStart + htmlTrEnd;
                messageBody = messageBody + htmlTableEnd;
                return messageBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
