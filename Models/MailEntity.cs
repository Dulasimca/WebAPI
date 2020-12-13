using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models
{
    public class MailEntity
    {
        public string FromMailid { get; set; }
        public string FromPassword { get; set; }
        public string ToMailid { get; set; }
        public string ToCC { get; set; }
        public int Port { get; set; }
        public string Subject { get; set; }
        public string BodyMessage { get; set; }
        public string SMTP { get; set; }
    }
}
