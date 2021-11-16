using BackEnd_GestaoFinanceira.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BackEnd_GestaoFinanceira.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class FilesUploadsController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;

        public FilesUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public string Upload([FromForm] FileUpload objectFile)
        {
            if (objectFile.files.Length > 0)
            {
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                var extension = Path.GetExtension(objectFile.files.FileName);
                var filePath = objectFile.Entrada + objectFile.idUsuario + extension;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = System.IO.File.Create(path + filePath))
                {
                    objectFile.files.CopyTo(fileStream);
                    fileStream.Flush();
                    return "Uploaded!";
                }
            }
            return "Not Uploaded!";
        }
    }
}
