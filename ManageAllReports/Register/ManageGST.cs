using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageAllReports.Register
{
    public class ManageGST
    {
        ManageReport report = new ManageReport();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public Tuple<bool,string> GenerateGSTFile(CommonEntity entity)
        {
            string fPath = string.Empty, subF_Path = string.Empty, fileName = string.Empty, filePath = string.Empty;
            StreamWriter streamWriter = null;
            try
            {
                fileName = entity.GCode + GlobalVariable.GSTFileName;
                fPath = GlobalVariable.ReportPath + "Reports";
                report.CreateFolderIfnotExists(fPath); // create a new folder if not exists
                subF_Path = fPath + "//" + entity.UserName; //ManageReport.GetDateForFolder();
                report.CreateFolderIfnotExists(subF_Path);
                //delete file if exists
                filePath = subF_Path + "//" + fileName + ".txt";
                report.DeleteFileIfExists(filePath);

                streamWriter = new StreamWriter(filePath, true);
                CreateGSTTextFile(streamWriter, entity);

                streamWriter.Flush();
                return new  Tuple<bool, string>(true,"GST File is Generated Sccessfully!");

            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message + " " + ex.StackTrace);
                return new Tuple<bool, string>(false, "GST File is not Generated!");
            }
            finally
            {
                streamWriter.Close();
                fPath = string.Empty; fileName = string.Empty;
                streamWriter = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="entity"></param>
        public void CreateGSTTextFile(StreamWriter sw, CommonEntity entity)
        {
            try
            {
                if (entity.Type == 2)
                {
                    foreach (DataRow row in entity.dataSet.Tables[1].Rows)
                    {
                        sw.WriteLine(row["GST"].ToString());
                    }
                }
                else
                {
                    foreach (DataRow row in entity.dataSet.Tables[0].Rows)
                    {
                        sw.WriteLine(row["GST"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
        }
    }
}
