using Microsoft.AspNetCore.Hosting;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.ImageSharp;

namespace ShamsAlShamoos01.Infrastructure.Services
{
    public class QrImageService
    {
        private readonly IWebHostEnvironment _env;
        private const int MaxChunkSize = 300;

        public QrImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // ---------------------------
        // 1️⃣ عکس → چند QR
        public List<string> GenerateQrsFromImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                return new List<string>();

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            return GenerateMultipleQrs(base64String);
        }

        // متد اصلی برای متن طولانی
        private List<string> GenerateMultipleQrs(string longText)
        {
            List<string> chunks = SplitText(longText, MaxChunkSize);
            List<string> fileNames = new();
            int partNumber = 1;

            string qrFilesPath = Path.Combine(_env.ContentRootPath, "QrFiles");
            if (!Directory.Exists(qrFilesPath))
                Directory.CreateDirectory(qrFilesPath);

            foreach (var chunk in chunks)
            {
                string fileName = $"qr_{DateTime.Now.Ticks}_{partNumber}.png";
                GenerateQrToFile(chunk, Path.Combine(qrFilesPath, fileName));
                fileNames.Add(fileName);
                partNumber++;
                Thread.Sleep(5); // جلوگیری از Tick یکسان
            }

            return fileNames;
        }

        private static List<string> SplitText(string text, int chunkSize)
        {
            List<string> parts = new();
            for (int i = 0; i < text.Length; i += chunkSize)
            {
                int length = Math.Min(chunkSize, text.Length - i);
                parts.Add(text.Substring(i, length));
            }
            return parts;
        }

        // ---------------------------
        // 2️⃣ ساخت QR و ذخیره به فایل
        private void GenerateQrToFile(string text, string filePath)
        {
            var qrData = new QRCodeGenerator().CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            byte[] pngBytes = pngQr.GetGraphic(20);
            File.WriteAllBytes(filePath, pngBytes);
        }

        // ---------------------------
        // 3️⃣ خواندن QRها و بازسازی عکس
        public void ReconstructImageFromQrs(List<string> qrFiles, string outputPath)
        {
            string combinedBase64 = "";
            var reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions { TryHarder = true }
            };


            foreach (var file in qrFiles)
            {
                if (!File.Exists(file)) continue;
                using var image = Image.Load<Rgba32>(file);
                var result = reader.Decode(image);
                if (result != null)
                    combinedBase64 += result.Text;
            }

            byte[] imageBytes = Convert.FromBase64String(combinedBase64);
            File.WriteAllBytes(outputPath, imageBytes);
        }
    }
    public class QrReaderService
    {
        public string ReadQrFromFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null; 
            }

            using var image = Image.Load<Rgba32>(filePath);

            // استفاده از BarcodeReader اختصاصی ImageSharp
            var reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true
                }
            };

            var result = reader.Decode(image);
            return result?.Text;
        }
    }
}
