using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.Models.Allotment
{
    public class AllotmentDataEntity
    {
      public string GCode;
      public string RCode;
      public string Taluk;
      public string AllotmentMonth;
      public string AllotmentYear;
      public List<ShopList> ShopItemList;
    }

    public class ItemEntity
    {
        public string ITCode;
        public string ITName;
        public float Quantity;
    }

    public class ShopList
    {
        public List<ItemEntity> ItemList;
        public string FPSCode;
        public string FPSName;
    }
}
