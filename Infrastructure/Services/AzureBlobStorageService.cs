using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class AzureBlobStorageService : IFileUploadService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, int? targetWidth = null, int? targetHeight = null)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset stream position

            if (targetWidth.HasValue && targetHeight.HasValue)
            {
                using var outputStream = new MemoryStream();
                using (var image = Image.Load(memoryStream))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(targetWidth.Value, targetHeight.Value),
                        Mode = ResizeMode.Crop
                    }));

                    await image.SaveAsJpegAsync(outputStream); // or SaveAsPngAsync
                }
                outputStream.Position = 0;

                await blobClient.UploadAsync(outputStream, new BlobHttpHeaders { ContentType = "image/jpeg" });
            }
            else
            {
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            return blobClient.Uri.ToString();
        }
    }
}
