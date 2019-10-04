using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace TNCSCAPI
{
    public class ManageReport
    {
        /// <summary>
        /// Format the string based on the length
        /// </summary>
        /// <param name="sValue">string value</param>
        /// <param name="length">Total Length</param>
        /// <param name="type">Format type 1- before,2-After</param>
        /// <returns></returns>
        public string StringFormat(string sValue, int length, int type = 0)
        {
            string result = string.Empty;
            string addvalues = string.Empty;
            bool isAddSpace = false;

            try
            {
                result = sValue;
                if (sValue.Length == length)
                {
                    result = sValue;
                }
                else if (sValue.Length > length)
                {
                    result = sValue.Substring(0, length);
                }
                else
                {
                    isAddSpace = true;
                    int addSpace = length - sValue.Length;
                    //  string
                    for (int i = 0; i < addSpace; i++)
                    {
                        addvalues = addvalues + " ";
                    }
                }
                if (isAddSpace)
                {
                    if (type == 1)
                    {
                        result = addvalues + result;
                    }
                    else if (type == 2)
                    {
                        result = result + addvalues;
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("StringFormat : " + ex.Message + " " + ex.InnerException);
            }
            finally
            {
                addvalues = string.Empty;
            }
            return result + "|";

        }

        /// <summary>
        /// Format the string based on the length
        /// </summary>
        /// <param name="sValue">string value</param>
        /// <param name="length">Total Length</param>
        /// <param name="type">Format type 1- before,2-After</param>
        /// <returns></returns>
        public string AddLine(int length)
        {
            string addvalues = string.Empty;
           try
            {
                    for (int i = 0; i < length; i++)
                    {
                        addvalues = addvalues + "-";
                    }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("AddLine : " + ex.Message + " " + ex.InnerException);
            }
            return addvalues + "|";

        }

        public Tuple<string,bool> StringFormatWithEmpty(string svalue)
        {
            try
            {
                if(!string.IsNullOrEmpty(svalue) && svalue!=null)
                {
                    return new Tuple<string, bool>(svalue,true);
                }
                return new Tuple<string, bool>(" ", false); 
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("StringFormatWithEmpty : " + ex.Message + " " + ex.InnerException);
                return new Tuple<string, bool>(" ", false); 
            }
        }

      
        /// <summary>
        /// Format the string based on the length
        /// </summary>
        /// <param name="sValue">string value</param>
        /// <param name="length">Total Length</param>
        /// <param name="type">Format type 1- before right allignment,2-After Left allignment</param>
        /// <returns></returns>
        public string StringFormatWithoutPipe(string sValue, int length, int type = 0)
        {
            string result = string.Empty;
            string addvalues = string.Empty;
            bool isAddSpace = false;

            try
            {
                result = sValue;
                if (sValue.Length == length)
                {
                    result = sValue;
                }
                else if (sValue.Length > length)
                {
                    result = sValue.Substring(0, length);
                }
                else
                {
                    isAddSpace = true;
                    int addSpace = length - sValue.Length;
                    //  string
                    for (int i = 0; i < addSpace; i++)
                    {
                        addvalues = addvalues + " ";
                    }
                }
                if (isAddSpace)
                {
                    if (type == 1)
                    {
                        result = addvalues + result;
                    }
                    else if (type == 2)
                    {
                        result = result + addvalues;
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("StringFormat : " + ex.Message + " " + ex.InnerException);
            }
            finally
            {
                addvalues = string.Empty;
            }
            return result + " ";

        }

        public string ConvertToUpper(string sValue)
        {
            try
            {
                return !string.IsNullOrEmpty(sValue) ? sValue.ToUpper() : sValue;
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(" ConvertToUpper : " + ex.Message + " " + ex.StackTrace);
                return sValue;
            }
        }

        /// <summary>
        /// Gets the empty space values only
        /// </summary>
        /// <param name="length">Length of space</param>
        /// <returns>Empty string</returns>
        public string AddSpace(int length)
        {
            string addvalues = string.Empty;
            for (int i = 0; i < length; i++)
            {
                addvalues = addvalues + " ";
            }
            return addvalues;
        }

        /// <summary>
        /// Change the decimal format for given values.
        /// </summary>
        /// <param name="sValues"></param>
        /// <returns></returns>
        public string Decimalformat(string sValues)
        {
            string sFormattedValue = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sValues))
                {
                    if (sValues.IndexOf(".") > 0)
                    {
                        string[] split = sValues.Split('.');
                        int length = Convert.ToString(split[1]).Length;
                        if (length == 1)
                        {
                            sFormattedValue = sValues + "0";
                        }
                        else
                        {
                            sFormattedValue = sValues;
                        }
                    }
                    else
                    {
                        sFormattedValue = sValues + ".00";
                    }
                }
                else
                {
                    sFormattedValue = "0.00";
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("Decimalformat : " + ex.Message + " " + ex.StackTrace);
                throw;
            }
            return sFormattedValue;
        }

        public decimal ConvertToDecimal(string sValue)
        {
            decimal dvalue = 0;
            if (!string.IsNullOrEmpty(sValue) && sValue != null)
            {
                dvalue = Convert.ToDecimal(sValue);
            }
            return dvalue;

        }

        /// <summary>
        /// Change the decimal format for given values.
        /// </summary>
        /// <param name="sValues"></param>
        /// <returns></returns>
        public string DecimalformatForWeight(string sValues)
        {
            string sFormattedValue = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sValues))
                {
                    if (sValues.IndexOf(".") > 0)
                    {
                        string[] split = sValues.Split('.');
                        int length = Convert.ToString(split[1]).Length;
                        if (length == 1)
                        {
                            sFormattedValue = sValues + "00";
                        }
                        else if (length == 2)
                        {
                            sFormattedValue = sValues + "0";
                        }
                        else
                        {
                            sFormattedValue = sValues;
                        }
                    }
                    else
                    {
                        sFormattedValue = sValues + ".000";
                    }
                }
                else
                {
                    sFormattedValue = "0.000";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sFormattedValue;
        }

        /// <summary>
        /// Create a new folder 
        /// </summary>
        /// <param name="Path">Folder path</param>
        public void CreateFolderIfnotExists(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        /// <summary>
        /// Delete file if exists.
        /// </summary>
        /// <param name="Path"></param>
        public void DeleteFileIfExists(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("DeleteFileIfExists " + ex.Message);
            }
           
        }

        /// <summary>
        /// Gets the folder for DateFormat
        /// </summary>
        /// <returns></returns>
        public static string GetDateForFolder()
        {
            return DateTime.Now.ToString("ddMMyyyy");
        }

        /// <summary>
        /// gets the current date
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd-MMM-yyyy");
        }

        /// <summary>
        /// Change the Date Format dd-MM-yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Format dd-MM-yyyy</returns>
        public string FormatDate(string date)
        {
            try
            {
                date = date.Replace("-", "/");
                if (date.Contains("00:00:00"))
                {
                    date = date.Replace("00:00:00", "");
                    date = date.Trim();
                    DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    return dt.ToString("dd-MMM-yyyy");
                }
                else
                {
                    DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    return dt.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("FormatDate : " + ex.Message + " " + ex.StackTrace);
                return " ";

            }

        }

        /// <summary>
        /// Change the Date Format dd-MM-yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Format dd-MM-yyyy</returns>
        public string FormatDirectDate(string date)
        {
            try
            {  
                if(!string.IsNullOrEmpty(date))
                {
                    return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                }
                return " ";
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("FormatDirectDate : " + ex.Message + " " + ex.StackTrace);
                return " ";

            }

        }


        /// <summary>
        /// Change the Date Format dd-MM-yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Format dd-MM-yyyy</returns>
        public string FormatIndianDate(string date)
        {
            try
            {
                date = date.Replace("-", "/");
                DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                return dt.ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("FormatIndianDate : " + ex.Message);
                return " ";
            }

        }

        /// <summary>
        /// Change the Date Format dd-MM-yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Format dd-MM-yyyy</returns>
        public string GetTime(string date)
        {
            try
            {
                date = date.Replace("-", "/");
                DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                return dt.ToString("hh:mm:ss tt");
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetTime : " + ex.Message);
                return " ";
            }

        }

        public string GetCurrentTime(DateTime dt)
        {
            try
            {
                return dt.ToString("hh:mm:ss tt");
            }
            catch (Exception)
            {
                return " ";
            }
        }


        /// <summary>
        /// Check the Data availability
        /// </summary>
        /// <param name="ds">dataset value</param>
        /// <returns></returns>
        public bool CheckDataAvailable(DataSet ds)
        {
            bool isAvailable = false;

            try
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isAvailable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("CheckData : " + ex.Message + " : " + ex.StackTrace);
            }
            return isAvailable;
        }

        /// <summary>
        /// Check the Data availability
        /// </summary>
        /// <param name="dt">Data Table</param>
        /// <returns></returns>
        public bool CheckDataAvailable(DataTable dt)
        {
            bool isAvailable = false;

            try
            {
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("CheckData : " + ex.Message + " : " + ex.StackTrace);
            }
            return isAvailable;
        }


        /// <summary>
        /// Change the Date Format dd-MM-yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Format dd-MM-yyyy</returns>
        public string GetDays(string FromDate,string ToDate)
        {
            try
            {
                TimeSpan different = Convert.ToDateTime(ToDate) - Convert.ToDateTime(FromDate);
                return different.Days.ToString();
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetDays : " + ex.Message);
                return " ";
            }

        }

        /// <summary>
        ///  Check the Data availability
        /// </summary>
        /// <param name="sValue">string value</param>
        /// <returns></returns>
        public bool CheckDataAvailable(string sValue)
        {
            bool isAvailable = false;

            try
            {
                if (!string.IsNullOrEmpty(sValue) && sValue != null)
                {
                    isAvailable = true;
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("CheckData : " + ex.Message + " : " + ex.StackTrace);
            }
            return isAvailable;
        }

        public List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public List<T> ConvertDataRowToList<T>(DataRow[] dataRow)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataRow)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr[column.ColumnName])) && dr[column.ColumnName] != null)
                    {
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null); //
                            else
                                continue;
                        }
                    }
                }
                return obj;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Add more content to display next line.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sData"></param>
        /// <param name="dataLength"></param>
        /// <param name="addSpace"></param>
        public int AddMoreContent(StreamWriter sw, string sData, int dataLength, int addSpace)
        {
            int numberofLines = 0;
            //Add all data for issuer
            if (sData.Length > dataLength)
            {
                int ilength = sData.Length - dataLength;
                int index = dataLength;
                int record = 0;

                while (ilength > 0)
                {
                    sw.Write(AddSpace(addSpace));
                    if (ilength >= dataLength)
                    {
                        record = dataLength;
                        ilength = ilength - dataLength;
                    }
                    else
                    {
                        record = ilength;
                        ilength = 0;
                    }
                    string sremainValue = sData.Substring(index, record);
                    sw.Write(sremainValue);
                    index = index + record;
                    sw.WriteLine("");
                    numberofLines++;
                }
            }
            return numberofLines;
        }
    }
}
