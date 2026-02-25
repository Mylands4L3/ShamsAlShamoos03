using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShamsAlShamoos03.Infrastructure.Services
{
    public class QrFileService
    {
        private readonly IWebHostEnvironment _env;

        public QrFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateMultipleQrs131(string base64String)
        {
            // مسیر ذخیره در wwwroot/uploads
            string folder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = $"qr_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            string outputPath = Path.Combine(folder, fileName);

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                File.WriteAllBytes(outputPath, imageBytes);
                return Path.Combine("uploads", fileName).Replace("\\", "/"); // مسیر برای img
            }
            catch (FormatException ex)
            {
                Console.WriteLine("رشته Base64 معتبر نیست: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
