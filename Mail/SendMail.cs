using System;
using System.Net;
using System.Net.Mail;
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
                messageBody += "Please provide the quotation for below mentioned products. <br><br>";
                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "Email id" + htmlTrEnd;
                messageBody += htmlTdStart + "Phone Number" + htmlTrEnd;
                messageBody += htmlTdStart + "Products" + htmlTrEnd;
                messageBody += htmlTdStart + "Remarks" + htmlTrEnd;
                messageBody += htmlHeaderRowEnd;
                //Loop all the rows from grid vew and added to html td  
                messageBody = messageBody + htmlTrStart + htmlTdStart;
                messageBody += bodyMessageEntity.Mailid + htmlTdEnd;
                messageBody += htmlTdStart + bodyMessageEntity.PhoneNumber + htmlTdEnd;
                messageBody += htmlTdStart + bodyMessageEntity.Products + htmlTdEnd;
                messageBody += htmlTdStart + bodyMessageEntity.Remarks + htmlTdEnd;
                messageBody += htmlTrEnd + htmlTableEnd + "<br><br>";
                messageBody += "Thanks & Regads <br>";
                messageBody += " SI Support Team";
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
