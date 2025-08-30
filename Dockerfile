# Stage 1: Build Environment
# نستخدم صورة الـ SDK الكاملة التي تحتوي على كل أدوات البناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# انسخ ملفات الـ .csproj لكل المشاريع وملف الـ solution
# هذه الخطوة مهمة للاستفادة من الـ caching في Docker
COPY ["AlaMashi.sln", "."]
COPY ["AlaMashi.API/AlaMashi.API.csproj", "AlaMashi.API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# قم باستعادة كل الـ dependencies (NuGet packages)
RUN dotnet restore "AlaMashi.sln"

# انسخ باقي ملفات الكود
COPY . .

# انتقل إلى مجلد مشروع الـ API وقم بعملية النشر (publish)
WORKDIR "/src/AlaMashi.API"
RUN dotnet publish "AlaMashi.API.csproj" -c Release -o /app/publish

# ---

# Stage 2: Runtime Environment
# نستخدم صورة الـ ASP.NET Runtime الخفيفة لتشغيل التطبيق
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# انسخ فقط الملفات المنشورة من مرحلة البناء
COPY --from=build /app/publish .

# تحديد البورت الذي سيعمل عليه التطبيق داخل الـ container
EXPOSE 8080

# الأمر الافتراضي لتشغيل التطبيق
ENTRYPOINT ["dotnet", "AlaMashi.API.dll"]