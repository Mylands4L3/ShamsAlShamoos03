using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShamsAlShamoos02.Infrastructure;
 using ShamsAlShamoos02.Infrastructure.Persistence.Contexts;
using ShamsAlShamoos02.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos02.Infrastructure.Persistence.UnitOfWork;
using ShamsAlShamoos02.Server.Services;
using ShamsAlShamoos02.Shared.Entities;
using Syncfusion.Licensing;
using System.Globalization;


//فارسی کردن
var culture = new CultureInfo("fa-IR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
//فارسی کردن

 


var licenseKey = "MTU4NUAzMjM3MkUzMTJFMzluT08wbzRnYm4zUlFDOVRzWVpYbUtuSEl0aUhTZmNMYjQxekhrV0NVRnlzPQ==";

SyncfusionLicenseProvider.RegisterLicense(licenseKey);


var builder = WebApplication.CreateBuilder(args);

// --------------------
// 1️⃣ Services
// --------------------

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (برای اینکه Client به سرور وصل شود)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Session + Cache
builder.Services.AddDistributedMemoryCache(); // برای Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUsers, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// UnitOfWork و Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDapperGenericRepository, DapperGenericRepository>();
builder.Services.AddScoped<APIDataService01>();
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper (اختیاری اگر داری)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --------------------
// 2️⃣ Build App
// --------------------
var app = builder.Build();


var qrFilesPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "QrFiles"));

if (!Directory.Exists(qrFilesPath))
{
    Directory.CreateDirectory(qrFilesPath);
}
 


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(qrFilesPath),
    RequestPath = "/QrFiles"
});


// --------------------
// 3️⃣ Middleware
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Serve Blazor framework files (_framework)
app.UseBlazorFrameworkFiles();
// CORS قبل از MapControllers
app.UseCors();

// Session middleware
app.UseSession();

app.MapControllers();
app.MapFallbackToFile("index.html");

// --------------------
// 4️⃣ Run
// --------------------
app.Run();
