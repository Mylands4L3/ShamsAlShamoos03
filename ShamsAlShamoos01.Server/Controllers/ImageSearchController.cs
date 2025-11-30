using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShamsAlShamoos01.Infrastructure.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShamsAlShamoos01.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageSearchController : ControllerBase
    {
        private readonly IImageSearchService _imageSearchService;

        public ImageSearchController(IImageSearchService imageSearchService)
        {
            _imageSearchService = imageSearchService;
        }

        [HttpPost("find-similar")]
        public async Task<IActionResult> FindSimilar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("هیچ فایلی آپلود نشده است.");

            // مسیر ذخیره موقت فایل آپلود شده
            string tempPath = Path.Combine(Path.GetTempPath(), file.FileName);
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // مسیر پوشه QrFiles
            //string folderPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "QrFiles"));
            string folderPath = @"D:\upload\indexImage1";


            // پیدا کردن عکس‌های مشابه
            var similarImages = _imageSearchService.FindSimilarImages(tempPath, folderPath);

            // پاک کردن فایل موقت
            System.IO.File.Delete(tempPath);

            return Ok(similarImages);
        }
    }
}
