using Microsoft.AspNetCore.Hosting;
using QRCoder;

namespace ShamsAlShamoos01.Infrastructure.Services;

public class QrCodeService
{
    private readonly IWebHostEnvironment _env;

    public QrCodeService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public string GenerateQrToFile(string text, string fileName)
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
