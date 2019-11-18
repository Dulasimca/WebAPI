using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.DataTransfer
{
    public class DataTransferEntity
    {
        public string GCode { get; set; }
        public string RCode { get; set; }
        public int DocType { get; set; }
        public int TripType { get; set; }
        public string DocNumber { get; set; }
    }
}
