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
        public int G2GStatus { get; set; }
        public int GPSStatus { get; set; }
        public string DocNumber { get; set; }
    }
}
