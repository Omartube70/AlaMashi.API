using Application.Common.Behaviors;
using Application.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// =================================================================================
// 1. تسجيل الخدمات في الـ DI Container (Services Registration)
// =================================================================================

// إضافة Controllers ودعم NewtonsoftJson للتحديثات الجزئية (PATCH)
builder.Services.AddControllers().AddNewtonsoftJson();

// تسجيل الـ DbContext مع قراءة الـ Connection String
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// تسجيل خدمات طبقة Infrastructure مع الواجهات الخاصة بها
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();

// تسجيل MediatR والـ Pipeline Behaviors بالترتيب الصحيح
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly); // Requires a dummy class in Application
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// تسجيل كل الـ Validators من مشروع الـ Application تلقائيًا
builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

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
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// الترتيب الصحيح: المصادقة أولًا ثم الصلاحيات
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();