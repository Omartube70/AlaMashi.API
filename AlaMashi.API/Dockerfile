# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# انسخ كل الكود من الـ repository دفعة واحدة
COPY . .

# اعمل restore لكل الـ dependencies في الـ solution بالكامل
RUN dotnet restore "AlaMashi.sln"

# انتقل لمجلد مشروع الـ API الرئيسي واعمل له publish
WORKDIR /src/AlaMashi.API
RUN dotnet publish -c Release -o /out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "AlaMashi.API.dll"]