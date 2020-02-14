using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageItemMasterController : ControllerBase
    {
        [HttpPost("{id}")]
        public bool Post(CommodityEntity commodityEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                if (commodityEntity.Type == 1)
                {
                    parameterList.Add(new KeyValuePair<string, string>("@ITCode", commodityEntity.ITCode));
                    parameterList.Add(new KeyValuePair<string, string>("@ITDescription", commodityEntity.ITDescription));
                    parameterList.Add(new KeyValuePair<string, string>("@ItemType", commodityEntity.ItemType));
                    return manageSQLConnection.InsertData("InsertItemMasterCereal", parameterList);
                }
                else
                {
                    parameterList.Add(new KeyValuePair<string, string>("@ITCode", commodityEntity.ITCode));
                    parameterList.Add(new KeyValuePair<string, string>("@ITDescription", commodityEntity.ITDescription));
                    parameterList.Add(new KeyValuePair<string, string>("@ITBweighment", commodityEntity.ITBweighment));
                    parameterList.Add(new KeyValuePair<string, string>("@ittax", commodityEntity.ittax));
                    parameterList.Add(new KeyValuePair<string, string>("@GRName", commodityEntity.GRName));
                    parameterList.Add(new KeyValuePair<string, string>("@ItemType", commodityEntity.ItemType));
                    parameterList.Add(new KeyValuePair<string, string>("@Hsncode", commodityEntity.Hsncode));
                    parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", commodityEntity.DeleteFlag));
                    parameterList.Add(new KeyValuePair<string, string>("@Activeflag", commodityEntity.ActiveFlag));
                    parameterList.Add(new KeyValuePair<string, string>("@Allotmentgroup", commodityEntity.Allotmentgroup));
                    parameterList.Add(new KeyValuePair<string, string>("@SFlag", Convert.ToString(commodityEntity.SFlag)));
                    parameterList.Add(new KeyValuePair<string, string>("@CBFlag", Convert.ToString(commodityEntity.CBFlag)));
                    parameterList.Add(new KeyValuePair<string, string>("@Unit", Convert.ToString(commodityEntity.Unit)));
                    return manageSQLConnection.InsertData("InsertItemMaster", parameterList);
                }
                //return manageSQLConnection.InsertData("InsertItemMaster", parameterList);
            }

            finally
            {
                parameterList = null;
            }
        }
        [HttpGet]
        public string Get(int ItemType)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            DataSet ds = new DataSet();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@Type", ItemType.ToString()));
                ds = manageSQLConnection.GetDataSetValues("GetItemMaster", parameterList);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class CommodityEntity
    {
        public int Type { get; set; }
        public string ITCode { get; set; }
        public string ITDescription { get; set; }
        public string ITBweighment { get; set; }
        public string ittax { get; set; }
        public string GRName { get; set; }
        public string ItemType { get; set; }
        public string Hsncode { get; set; }
        public string DeleteFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string Allotmentgroup { get; set; }
        public bool SFlag { get; set; }
        public bool CBFlag { get; set; }
        public string Unit { get; set; }
    }
}