using Microsoft.AspNetCore.Http;

namespace BackEnd_GestaoFinanceira.Model
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
