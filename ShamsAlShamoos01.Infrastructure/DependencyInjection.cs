using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShamsAlShamoos01.Infrastructure.Services;

namespace ShamsAlShamoos01.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔹 ثبت سرویس‌های Infrastructure
            services.AddScoped<QrCodeService>();
            services.AddScoped<QrBatchService>();

            // 🔹 ثبت دیگر سرویس‌ها مثل Repository و UnitOfWork اگر داری
            // services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
