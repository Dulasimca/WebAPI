using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI
{
    public class AuditLog
    {
        public static void WriteError(string sMessage)
        {
            string fPath = string.Empty, sPath = string.Empty, sFileName = string.Empty;
            try
            {
                fPath = AppDomain.CurrentDomain.BaseDirectory + "//LogFiles";
                if (!Directory.Exists(fPath))
                {
                    Directory.CreateDirectory(fPath);
                }
                sFileName = DateTime.Now.ToString("ddMMyyyy");
                StreamWriter sw = new StreamWriter(fPath + "//" + sFileName + ".txt", true);
                sw.WriteLine(DateTime.Now + " : " + sMessage);
                sw.Flush();
                sw.Close();          
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fPath = string.Empty; sFileName = string.Empty;
            }
        }
    }
}
