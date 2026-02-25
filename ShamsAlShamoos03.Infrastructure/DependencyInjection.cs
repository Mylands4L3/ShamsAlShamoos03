using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShamsAlShamoos03.Infrastructure.Services;

namespace ShamsAlShamoos03.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔹 ثبت سرویس‌های Infrastructure
            services.AddScoped<QrCodeService>();
            services.AddScoped<QrBatchService>();
            services.AddScoped<QrFileService>();
            services.AddScoped<Services.IImageSearchService, Services.ImageSearchService>();
            services.AddSingleton<IFaceRecognitionService, FaceRecognitionService>();
            return services;
        }

    }
}
