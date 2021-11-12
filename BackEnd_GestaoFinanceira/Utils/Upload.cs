using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Utils
{
    public class Upload
    {
        public string UploadFile(IFormFile file, int idFuncionario)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = new string(idFuncionario.ToString());
                    fileName = fileName + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return fileName;
                }

                else
                {
                    return "";
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
