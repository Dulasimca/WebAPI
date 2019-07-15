using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TNCSCAPI.Models
{
    public class ManageDashboard
    {
        /// <summary>
        /// Gets Particular rows in a table.
        /// </summary>
        /// <param name="RegionName">Region Name</param>
        /// <param name="MajorItemName">Major Item Name</param>
        /// <param name="dataTable">Data table value</param>
        /// <returns></returns>
        public decimal[] GetValueInArray(string MajorItemName, DataTable dataTable, string[] Regioninfo)
        {
            decimal[] svalues = new decimal[Regioninfo.Length];
            int i = 0;
            foreach (var item in Regioninfo)
            {
                DataRow[] rowsFiltered = dataTable.Select("RGNAME='" + item + "' and MajorName='" + MajorItemName + "'");
                //FilterDataRow(item, "PALMOLIEN POUCH", dataTable);
                //ds.Tables[0].Select("RGNAME='" + item + "' and MajorName='RAW GRADEA'");
                if (rowsFiltered != null)
                {
                    if (rowsFiltered.Length > 0)
                    {
                        svalues[i] = Convert.ToDecimal(rowsFiltered[0][2]);
                    }
                    else
                    {
                        svalues[i] = 0;
                    }
                }
                else
                {
                    svalues[i] = 0;
                }
                i++;
            }
            return svalues;
        }

        public List<object> MasterData(DataSet ds)
        {
            List<object> list = new List<object>();
            if (ds.Tables.Count > 0)
            {
                list.Add(ds.Tables[0].Rows[0][0]);
                list.Add(ds.Tables[1].Rows[0][0]);
                list.Add(ds.Tables[2].Rows[0][0]);
                list.Add(ds.Tables[3].Rows[0][0]);
                list.Add(ds.Tables[4].Rows[0][0]);
                list.Add(ds.Tables[5].Rows[0][0]);
                list.Add(ds.Tables[6].Rows[0][0]);
                list.Add(ds.Tables[7].Rows[0][0]);
                list.Add(ds.Tables[8].Rows[0][0]);
                string[] notifications = ds.Tables[9].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                list.Add(notifications);
            }
            return list;
        }
    }
}
