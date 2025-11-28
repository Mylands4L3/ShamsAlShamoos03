using Microsoft.AspNetCore.Hosting;
using QRCoder;

namespace ShamsAlShamoos01.Infrastructure.Services;

public class QrBatchService
{
    private readonly IWebHostEnvironment _env;

    public QrBatchService(IWebHostEnvironment env)
    {
        _env = env;
    }

    private byte[] GenerateQrBytes(string text)
    {
        var qrData = QRCodeGenerator.GenerateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var pngQr = new PngByteQRCode(qrData);
        return pngQr.GetGraphic(20);
    }
}
