using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _uploadsPath;
        private readonly string _baseUrl;

        public LocalFileStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            var rootPath = Directory.GetCurrentDirectory();
            _uploadsPath = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(_uploadsPath))
                Directory.CreateDirectory(_uploadsPath);

            _baseUrl = config["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "https://alamashi.runasp.net";
        }

        public async Task<(byte[] FileBytes, string ContentType)> GetFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadsPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var contentType = "application/octet-stream";
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            provider.TryGetContentType(filePath, out contentType);

            var bytes = await File.ReadAllBytesAsync(filePath);

            return (bytes, contentType);
        }

        public async Task<string> UploadFileAsync(IFormFile file, int? targetWidth = null, int? targetHeight = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!Array.Exists(allowed, e => e == extension))
                throw new ArgumentException("Unsupported file type.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadsPath, fileName);

            if (targetWidth.HasValue && targetHeight.HasValue)
            {
                using var image = Image.Load(file.OpenReadStream());
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(targetWidth.Value, targetHeight.Value),
                    Mode = ResizeMode.Crop
                }));
                await image.SaveAsJpegAsync(filePath);
            }
            else
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            var url = $"{_baseUrl}/api/File/{fileName}";
            return url;
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) 
                return;

            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_uploadsPath, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await Task.CompletedTask;
        }
    }
}
