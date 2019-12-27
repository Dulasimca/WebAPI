using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TNCSCAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = GlobalVariable.ImagePath;
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    //ManageSQLConnection manageSQL = new ManageSQLConnection();
                    //sqlParameters.Add(new KeyValuePair<string, string>("@ID", ID));
                    //sqlParameters.Add(new KeyValuePair<string, string>("@Notes", notification.Notes));
                    //sqlParameters.Add(new KeyValuePair<string, string>("@Reason", notification.Reason));
                    //sqlParameters.Add(new KeyValuePair<string, string>("@isActive", notification.isActive));
                    //sqlParameters.Add(new KeyValuePair<string, string>("@ImageName", fileName));
                    //manageSQL.InsertData("InsertNotificationPopup", sqlParameters);
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }
    }

    public class NotificationEntity
    {
        public string ID { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
        public string isActive { get; set; }
    }
}