using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TNCSCAPI.Controllers.MasterDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageDepositorsMasterController : ControllerBase
    {

        [HttpPost("{id}")]
        public bool Post(DepositorEntity depositorEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> parameterList = new List<KeyValuePair<string, string>>();
            try
            {
                parameterList.Add(new KeyValuePair<string, string>("@DepositorCode", depositorEntity.DepositorCode));
                parameterList.Add(new KeyValuePair<string, string>("@DepositorName", depositorEntity.DepositorName));
                parameterList.Add(new KeyValuePair<string, string>("@DepositorType", depositorEntity.DepositorType));
                parameterList.Add(new KeyValuePair<string, string>("@RCODE", depositorEntity.RCode));
                parameterList.Add(new KeyValuePair<string, string>("@GODOWNCODE", depositorEntity.GCode));
                parameterList.Add(new KeyValuePair<string, string>("@DeleteFlag", depositorEntity.DeleteFlag));
                parameterList.Add(new KeyValuePair<string, string>("@ActiveFlag", depositorEntity.ActiveFlag));
                return manageSQLConnection.InsertData("InsertDepositorsMaster", parameterList);
            }
            finally
            {
                parameterList = null;
            }
        }
    }
    public class DepositorEntity
    {
        public string DepositorCode { get; set; }
        public string DepositorName { get; set; }
        public string DepositorType { get; set; }
        public string RCode { get; set; }
        public string GCode { get; set; }
        public string DeleteFlag { get; set; }
        public string ActiveFlag { get; set; }
    }
}