# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# انسخ ملفات الـ .csproj لكل المشاريع وملف الـ solution.
# ده مهم جدا عشان الـ "dotnet restore" يقدر يتعرف على كل التبعيات (dependencies)
COPY ["AlaMashi.API/AlaMashi.API.csproj", "AlaMashi.API/"]
COPY ["AlaMashi.BLL/AlaMashi.BLL.csproj", "AlaMashi.BLL/"]
COPY ["AlaMashi.DAL/AlaMashi.DAL.csproj", "AlaMashi.DAL/"]
COPY ["../AlaMashi.sln", "."]


# اعمل restore للـ dependencies للمشاريع كلها
RUN dotnet restore "AlaMashi.sln"

# انسخ كل ملفات الكود الأخرى
COPY . .

# اعمل publish لمشروع الـ API الرئيسي
WORKDIR /src/AlaMashi.API
RUN dotnet publish -c Release -o /out

# Stage 2: Run
# استخدم image أصغر وأكثر أمانا لتشغيل التطبيق
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "AlaMashi.API.dll"]