using Microsoft.AspNetCore.Mvc;
using ShamsAlShamoos02.Infrastructure.Services;
using Syncfusion.EJ2.Inputs;

namespace ShamsAlShamoos02.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceSearchController : ControllerBase
    {
        private readonly IFaceRecognitionService _faceService;
        private readonly IWebHostEnvironment _env;

        public FaceSearchController(IFaceRecognitionService faceService, IWebHostEnvironment env)
        {
            _faceService = faceService;
            _env = env;
        }

        [HttpPost("search")]
        public IActionResult Search([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            // مسیر موقت ذخیره فایل آپلود شده
            var tempFilePath = Path.Combine(Path.GetTempPath(), imageFile.FileName);
            using (var stream = System.IO.File.Create(tempFilePath))
            {
                imageFile.CopyTo(stream);
            }

            // مسیر فولدر تصاویر برای جستجو
            string folderPath = @"D:\upload\indexImage1";

            // مسیر فولدر نتیجه
            string resultFolder = @"D:\upload\Result01";
            if (!Directory.Exists(resultFolder))
            {
                Directory.CreateDirectory(resultFolder);

            }

            // جستجوی تصاویر مشابه
            var matches = _faceService.FindSimilarFaces(tempFilePath, folderPath);

            // کپی فایل‌های مشابه به پوشه نتیجه
            foreach (var matchPath in matches)
            {
                string fileName = Path.GetFileName(matchPath);
                string destPath = Path.Combine(resultFolder, fileName);

                // اگر فایل قبلاً وجود داشت، می‌توانید overwrite کنید یا skip
                System.IO.File.Copy(matchPath, destPath, overwrite: true);
            }

            // پاک کردن فایل موقت
            System.IO.File.Delete(tempFilePath);

            // برگرداندن نام فایل‌ها
            return Ok(matches.Select(p => Path.GetFileName(p)));
        }
    }
}
