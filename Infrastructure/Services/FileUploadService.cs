using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public FileUploadService(IConfiguration configuration)
        {
            var accessKey = configuration["AWS:AccessKeyID"];
            var secretKey = configuration["AWS:SecretAccessKey"];
            _bucketName = configuration["AWS:BucketName"];
            _region = configuration["AWS:Region"];

            _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(_region));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = uniqueFileName, 
                InputStream = memoryStream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead 
            };

            await _s3Client.PutObjectAsync(request);

            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{uniqueFileName}";
        }
    }
}
