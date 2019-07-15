using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TNCSCAPI.Models
{
    public class ManageMenu
    {
        /// <summary>
        /// Get the menu tree values
        /// </summary>
        /// <param name="list">list of all menu details</param>
        /// <param name="parent">parent</param>
        /// <returns></returns>
        public List<Menu> GetMenuTree(List<Menu> list, int? parent)
        {
            try
            {
                return list.Where(x => x.parentId == parent).Select(x => new Menu
                {
                    ID = x.ID,
                    label = x.label,
                    parentId = x.parentId,
                    routerLink = x.routerLink,
                    isActive = x.isActive,
                    items = GetMenuTree(list, x.ID)
                }).ToList();
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public List<Menu> ConvertDataTableToList(DataTable dataTable)
        {
            List<Menu> menus = new List<Menu>();
            if (dataTable.Rows.Count > 0)
            {
                menus = (from DataRow dr in dataTable.Rows
                         select new Menu()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             label = Convert.ToString(dr["Name"]),
                             parentId = Convert.ToInt32(dr["ParentId"]),
                             RoleId = Convert.ToInt32(dr["RoleId"]),
                             routerLink = Convert.ToString(dr["URL"]),
                             isActive = Convert.ToBoolean(dr["IsActive"])
                         }).ToList();

            }
            return menus;
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }




    }
    public class Menu
    {
        public int ID { get; set; }
        public string label { get; set; }
        public string routerLink { get; set; }
        public int? parentId { get; set; }
        public bool isActive { get; set; }
        public int RoleId { get; set; }
        public List<Menu> items { get; set; }
    }


}
