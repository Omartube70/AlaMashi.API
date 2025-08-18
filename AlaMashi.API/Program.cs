using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// --- 1. قراءة الإعدادات والتحقق منها ---
var jwtKey = configuration["Jwt:Key"] ??
           throw new InvalidOperationException("JWT Key not found in configuration.");
var jwtIssuer = configuration["Jwt:Issuer"] ??
              throw new InvalidOperationException("JWT Issuer not found in configuration.");

// --- 2. تسجيل الخدمات (Services) ---
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddControllers().AddNewtonsoftJson();

// إضافة خدمة المصادقة للـ API (تبقى كما هي)
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
        ValidIssuer = jwtIssuer,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// ⬇️⬇️⬇️  هذا هو التعديل  ⬇️⬇️⬇️
// إضافة خدمات Swagger بالشكل الأساسي فقط
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 3. تهيئة مسار الطلبات (Pipeline) ---
app.UseMiddleware<ErrorHandlingMiddleware>();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

// الترتيب الصحيح للمصادقة والصلاحيات (تبقى كما هي)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();