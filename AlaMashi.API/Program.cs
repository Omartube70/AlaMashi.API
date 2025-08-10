// باستخدام الإضافات الجديدة
using AlaMashi.BLL;
using AlaMashi.DAL;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// 1. قراءة سلسلة الاتصال من متغيرات البيئة أو appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. تسجيل الخدمات (Services)

// تسجيل UserDAL كـ خدمة (Service)
builder.Services.AddScoped<UserDAL>(provider => new UserDAL(connectionString));

// تسجيل JwtService كـ خدمة
builder.Services.AddScoped<JwtService>();

// إضافة خدمات MVC (Controllers) إلى الحاوية
builder.Services.AddControllers();

// إضافة خدمات Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// تهيئة مسار طلبات HTTP
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
