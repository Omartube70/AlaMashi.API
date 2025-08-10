// باستخدام الإضافات الجديدة
using AlaMashi.BLL;
using AlaMashi.DAL;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// --- ADDITIONS START HERE ---

// 1. قراءة الـ Connection String من ملف appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. تسجيل الخدمات (Services)

// تسجيل UserDAL كـ خدمة (Service)
// يتم إنشاء نسخة جديدة من UserDAL لكل طلب (scoped)
// constructor الخاص بـ UserDAL يحتاج إلى connectionString، لذلك نمرره هنا.
builder.Services.AddScoped<UserDAL>(provider => new UserDAL(connectionString));

// تسجيل JwtService كـ خدمة
// builder.Configuration.GetSection("Jwt") يقرأ قسم الـ Jwt من ملف appsettings.json
// ثم يمرره إلى الـ constructor الخاص بـ JwtService
builder.Services.AddScoped<JwtService>();

// --- ADDITIONS END HERE ---


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