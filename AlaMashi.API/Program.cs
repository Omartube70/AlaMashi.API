using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// =================================================================================
// 1. تكوين HTTPS و Forwarded Headers للعمل خلف Reverse Proxy
// =================================================================================

// ✅ تفعيل HTTPS Redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = 443;
});

// ✅ تكوين Forwarded Headers لدعم Reverse Proxy (مهم جداً لـ MonsterASP)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                              ForwardedHeaders.XForwardedProto |
                              ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// =================================================================================
// 2. تسجيل الخدمات في الـ DI Container
// =================================================================================

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }
    )
);

// تسجيل خدمات طبقة Infrastructure
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

// تسجيل MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

// تسجيل الـ Pipeline Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// إعداد المصادقة باستخدام JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ✅ تحديث CORS للسماح بـ HTTPS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("*");
    });
});

// =================================================================================
// 3. بناء التطبيق وتهيئة Pipeline
// =================================================================================

var app = builder.Build();

// ✅ تفعيل Forwarded Headers في بداية 
app.UseForwardedHeaders();

// معالجة الأخطاء
app.UseMiddleware<ErrorHandlingMiddleware>();

// ✅ إعادة توجيه HTTP إلى HTTPS
app.UseHttpsRedirection();

app.UseRouting();

// ✅ تفعيل CORS
app.UseCors("AllowAll");

app.UseAuthentication();
    app.Use(async (context, next) =>
    {
        // إذا كان الطلب للصور، اسمح بالوصول بدون Authentication
        if (context.Request.Path.StartsWithSegments("/api/File") ||
            context.Request.Path.StartsWithSegments("/uploads") ||
            context.Request.Path.StartsWithSegments("/images"))
        {
            // Skip authentication for file requests
            await next();
            return;
        }

        await next();
    });
app.UseAuthorization();

// ✅ Static Files مع Cache
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
        // ✅ إضافة Security Headers
        ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        ctx.Context.Response.Headers.Append("X-Frame-Options", "DENY");
        ctx.Context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    }
});

app.MapControllers();

app.Run();