using Microsoft.AspNetCore.Mvc;
using ShamsAlShamoos01.Infrastructure.Services;
using System.IO;
using System.Linq;

namespace ShamsAlShamoos01.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrController : ControllerBase
    {
        private readonly QrCodeService _qr;
        private readonly QrBatchService _qrBatch;

        public QrController(QrCodeService qr, QrBatchService qrBatch)
        {
            _qr = qr;
            _qrBatch = qrBatch;
        }

        #region Helper Methods

        private string GenerateQrFile(string text)
        {
            string fileName = $"qr_{DateTime.Now.Ticks}";
            _qr.GenerateQrToFile(text, fileName);
            return fileName;
        }

        private string GetQrFolderPath()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "QrFiles"));
        }

        private string GetFullFilePath(string fileName)
        {
            return Path.Combine(GetQrFolderPath(), fileName + ".png");
        }

        #endregion

        [HttpGet("GetAllFiles")]
        public IActionResult GetAllFiles()
        {
            string qrFilesPath = GetQrFolderPath();

            if (!Directory.Exists(qrFilesPath))
            {
                return Ok(new string[] { });
            }

            var files = Directory.GetFiles(qrFilesPath, "*.png")
                                 .Select(f => Path.GetFileNameWithoutExtension(f))
                                 .ToList();

            return Ok(files);
        }

        [HttpGet("Generate")]
        [HttpGet("Generate1")]
        public IActionResult GenerateQr([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Text cannot be empty");
            }

            string fileName = GenerateQrFile(text);
            return Ok(fileName);
        }

        [HttpGet("ReadQr")]
        public IActionResult ReadQr([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("FileName cannot be empty");
            }

            string filePath = GetFullFilePath(fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {filePath}");
            }

            try
            {
                var qrReader = new QrReaderService();
                var text = qrReader.ReadQrFromFile(filePath);

                if (string.IsNullOrEmpty(text))
                {
                    return BadRequest("خواندن متن QR ناموفق بود");
                }

                return Ok(text);
            }
            catch (Exception ex)
            {
                return BadRequest($"خطا در خواندن QR: {ex.Message}");
            }
        }

        [HttpPost("GenerateBatch")]
        public IActionResult GenerateBatch([FromBody] string longText)
        {
            if (string.IsNullOrWhiteSpace(longText))
            {
                return BadRequest("Text cannot be empty");
            }

            var files = _qrBatch.GenerateMultipleQrs(longText);
            return Ok(files);
        }

        [HttpGet("GetQrFile")]
        public IActionResult GetQrFile([FromQuery] string fileName)
        {
            string solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\.."));
            string filePath = Path.Combine(solutionRoot, "QrFiles", fileName + ".png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {filePath}");
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "image/png");
        }
    }
}
