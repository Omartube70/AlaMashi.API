using Microsoft.Data.SqlClient; // تأكد من إضافة هذه الحزمة

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // يمكن أن يظل هنا أو يتم نقله

var app = builder.Build();

// Configuration for Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Rest of the code...
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();