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
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// =================================================================================
// 1. تسجيل الخدمات في الـ DI Container (Services Registration)
// =================================================================================

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// إضافة Controllers ودعم NewtonsoftJson للتحديثات الجزئية (PATCH)
builder.Services.AddControllers().AddNewtonsoftJson();

// تسجيل الـ DbContext مع قراءة الـ Connection String
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }
    )
);


// تسجيل خدمات طبقة Infrastructure مع الواجهات الخاصة بها
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
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
// تسجيل MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly);
});

// تسجيل كل الـ Validators من مشروع الـ Application تلقائيًا
builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

// تسجيل الـ Pipeline Behaviors مباشرة في الخدمات
// الترتيب هنا مهم: السلوك المسجل أولاً يتنفذ أولاً (يكون في الطبقة الخارجية)
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// إعداد Swagger مع دعم JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AlaMashi API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// =================================================================================
// 2. بناء التطبيق (Build the App)
// =================================================================================
var app = builder.Build();

// =================================================================================
// 3. تهيئة مسار الطلبات (HTTP Request Pipeline)
// =================================================================================

// استخدام الـ Middleware لمعالجة الأخطاء في بداية الـ Pipeline
app.UseMiddleware<ErrorHandlingMiddleware>();

// تفعيل Swagger فقط في بيئة التطوير للأمان
//if (app.Environment.IsDevelopment())
{
 app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlaMashi API v1");
        c.RoutePrefix = "swagger";
    });
}


app.UseHttpsRedirection();
app.UseRouting();

// ✅ تفعيل سياسة CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


// ✅ السماح بعرض الملفات من wwwroot/uploads
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true, // علشان لو نوع الملف مش معروف
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});

app.MapControllers();

app.Run();
