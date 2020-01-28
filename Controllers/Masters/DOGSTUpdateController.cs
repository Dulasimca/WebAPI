using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TNCSCAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOGSTUpdateController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(IssuerGSTEntity issuerGSTEntity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@GCode", issuerGSTEntity.GCode));
            sqlParameters.Add(new KeyValuePair<string, string>("@GSTNumber", issuerGSTEntity.GSTNumber));
            sqlParameters.Add(new KeyValuePair<string, string>("@IssuerCode", issuerGSTEntity.IssuerCode));
            manageSQLConnection.InsertData("UpdateGSTInIssuerMaster", sqlParameters);
            return new Tuple<bool, string>(true, "Updated Successfully!");
        }


    }

    public class IssuerGSTEntity
    {
        public string GSTNumber { get; set; }
        public string GCode { get; set; }
        public string IssuerCode { get; set; }
    }
}