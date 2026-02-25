using Microsoft.AspNetCore.Hosting;
using QRCoder;
using Tesseract;

namespace ShamsAlShamoos01.Infrastructure.Services;

public class QrCodeService
{
    private readonly IWebHostEnvironment _env;

    public QrCodeService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public string ImageToBase64(string imagePath)
    {
        // خواندن کل تصویر به آرایه بایت
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

        // تبدیل بایت‌ها به رشته Base64
        return Convert.ToBase64String(imageBytes);
    }
    public void Base64ToImage(string base64String, string outputPath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        System.IO.File.WriteAllBytes(outputPath, imageBytes);
    }

    public string ImageToText(string imagePath)
    {
        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);
        return page.GetText();
    }
    public List<string> SplitText(string text, int maxLength = 2000)
    {
        var parts = new List<string>();
        for (int i = 0; i < text.Length; i += maxLength)
        {
            parts.Add(text.Substring(i, Math.Min(maxLength, text.Length - i)));
        }
        return parts;
    }

    public string GenerateQrBase64(string text)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);

        var bytes = qrCode.GetGraphic(20);
        return Convert.ToBase64String(bytes);
    }

    public string GenerateQrToFile(string text, string folderName, string fileName)
    {
        var qrData = QRCodeGenerator.GenerateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var pngQr = new PngByteQRCode(qrData);
        byte[] pngBytes = pngQr.GetGraphic(20);

        string basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "QrFiles",
            folderName
        );

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        string filePath = Path.Combine(basePath, $"{fileName}.png");
        File.WriteAllBytes(filePath, pngBytes);

        // مسیر نسبی برای دیتابیس
        return Path.Combine(folderName, $"{fileName}.png");
    }

    public string GenerateQrToFile111(string text, string fileName)
    {
        var qrData = QRCodeGenerator.GenerateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var pngQr = new PngByteQRCode(qrData);
        byte[] pngBytes = pngQr.GetGraphic(20);

        // مسیر یکسان با StaticFiles
        string qrFilesPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "QrFiles"));

        if (!Directory.Exists(qrFilesPath))
        {
            Directory.CreateDirectory(qrFilesPath);
        }


        string filePath = Path.Combine(qrFilesPath, $"{fileName}.png");
        File.WriteAllBytes(filePath, pngBytes);

        return fileName; // فقط نام فایل را برگردان
    }



}
