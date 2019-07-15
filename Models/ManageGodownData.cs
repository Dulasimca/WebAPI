using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Models
{
    public class ManageGodownData
    {
        public List<GodownData> GetGodownDatas(DataSet ds)
        {
            List<GodownData> _godownDatas = new List<GodownData>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GodownData _godowndata = new GodownData();
                        _godowndata.Code = Convert.ToString(dr["TNCSRegn"]);
                        _godowndata.Name = Convert.ToString(dr["RGNAME"]);
                        _godowndata.Capacity = Convert.ToDecimal(dr["TNCSCapacity"]);
                        _godowndata.Carpet = Convert.ToDecimal(dr["TNCSCarpet"]);
                        _godowndata.list = GetGodowns(ds.Tables[1], Convert.ToString(dr["TNCSRegn"]));
                        _godownDatas.Add(_godowndata);
                    }
                }
                return _godownDatas;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<GodownData> GetGodowns(DataTable dt, string regionCode)
        {
            List<GodownData> _godownDatas = new List<GodownData>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dataRows = dt.Select("TNCSRegn='" + regionCode + "'");

                    foreach (DataRow dr in dataRows)
                    {
                        GodownData _godowndata = new GodownData();
                        _godowndata.Code = Convert.ToString(dr["TNCSRegn"]);
                        _godowndata.Name = Convert.ToString(dr["TNCSName"]);
                        _godowndata.Capacity = Convert.ToDecimal(dr["TNCSCapacity"]);
                        _godowndata.Carpet = Convert.ToDecimal(dr["TNCSCarpet"]);
                        _godowndata.GCode = Convert.ToString(dr["TNCSCode"]);
                        _godownDatas.Add(_godowndata);
                    }
                }
                return _godownDatas;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public class GodownData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public decimal Carpet { get; set; }
        public string GCode { get; set; }
        public List<GodownData> list { get; set; }
    }
}
