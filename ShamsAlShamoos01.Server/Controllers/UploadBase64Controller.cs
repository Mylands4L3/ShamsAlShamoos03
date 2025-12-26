using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.WebRequestMethods;

namespace ShamsAlShamoos01.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadBase64Controller : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadBase64Controller(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromBody] Base64UploadRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Base64String))
                return BadRequest("Base64 string is empty");

            try
            {
                // مسیر ذخیره در wwwroot/uploads
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = $"qr_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string outputPath = Path.Combine(folder, fileName);

                byte[] imageBytes = Convert.FromBase64String(request.Base64String);
                System.IO.File.WriteAllBytes(outputPath, imageBytes);

                // برگرداندن مسیر نسبی برای نمایش در مرورگر
                string relativePath = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";


                

                return Ok(new { ImagePath = relativePath });
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string");
            }
        }
    }

    public class Base64UploadRequest
    {
        public string Base64String { get; set; } = string.Empty;
    }
}
