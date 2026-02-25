using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using ShamsAlShamoos03.Infrastructure.Services;

namespace ShamsAlShamoos03.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly QrCodeService _qrService;
        private readonly QrBatchService _qrBatch;

        public UploadController(IWebHostEnvironment env, QrCodeService qrService, QrBatchService qrBatch)
        {
            _env = env;
            _qrService = qrService;
            _qrBatch = qrBatch;

        }


        [HttpPost("image-to-qr")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file");

            string extractedText;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                extractedText = Convert.ToBase64String(ms.ToArray());
            }

            // تولید QRها
            var generatedFiles = _qrBatch.GenerateMultipleQrs(extractedText);
            // برگرداندن نام فایل‌ها به کلاینت
            return Ok(generatedFiles);

        }



    }
}
