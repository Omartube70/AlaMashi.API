using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<(byte[] FileBytes, string ContentType)> GetFileAsync(string fileName);

        Task<string> UploadFileAsync(IFormFile file, int? targetWidth = null, int? targetHeight = null);

        public Task DeleteFileAsync(string fileUrl);
    }
}
