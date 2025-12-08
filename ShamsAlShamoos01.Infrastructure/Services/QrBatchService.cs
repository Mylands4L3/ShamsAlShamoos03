using Microsoft.AspNetCore.Hosting;
using QRCoder;

namespace ShamsAlShamoos01.Infrastructure.Services;

public class QrBatchService
{
    private readonly IWebHostEnvironment _env; 
    private readonly QrCodeService _qrCodeService;

    // حداکثر کاراکتر در هر QR
    private const int MaxChunkSize = 300;
    public QrBatchService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public List<string> GenerateMultipleQrs(string longText)
    {
        if (string.IsNullOrWhiteSpace(longText))
            return new List<string>();

        List<string> chunks = SplitText(longText, MaxChunkSize);
        List<string> fileNames = new();

        int partNumber = 1;

        foreach (var chunk in chunks)
        {
            string fileName = $"qr_{DateTime.Now.Ticks}_{partNumber}";
            _qrCodeService.GenerateQrToFile(chunk, fileName);
            fileNames.Add(fileName);
            partNumber++;
            Thread.Sleep(5); // جلوگیری از تولید Tick یکسان
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
    private byte[] GenerateQrBytes(string text)
    {
        var qrData = QRCodeGenerator.GenerateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var pngQr = new PngByteQRCode(qrData);
        return pngQr.GetGraphic(20);
    }
}
