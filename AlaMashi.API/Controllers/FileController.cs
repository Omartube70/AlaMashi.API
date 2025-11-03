using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlaMashi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// جلب الصورة بدون Authentication - متاح للجميع
        /// </summary>
        [HttpGet("{fileName}")]
        [AllowAnonymous] // ✅ مهم جداً - يسمح بالوصول بدون تسجيل دخول
        public async Task<IActionResult> GetFile(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest(new { status = "error", message = "File name is required" });
                }

                var (fileBytes, contentType) = await _fileStorageService.GetFileAsync(fileName);

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    return NotFound(new { status = "error", message = "File not found" });
                }

                // إضافة Cache Headers لتحسين الأداء
                Response.Headers.Add("Cache-Control", "public, max-age=31536000"); // سنة
                Response.Headers.Add("Expires", DateTime.UtcNow.AddYears(1).ToString("R"));

                return File(fileBytes, contentType);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { status = "error", message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Failed to retrieve file", details = ex.Message });
            }
        }

        /// <summary>
        /// جلب الصورة من المسار الكامل - بدون Authentication
        /// </summary>
        [HttpGet("path/{*filePath}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileByPath(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return BadRequest(new { status = "error", message = "File path is required" });
                }

                // استخراج اسم الملف من المسار
                var fileName = Path.GetFileName(filePath);

                var (fileBytes, contentType) = await _fileStorageService.GetFileAsync(fileName);

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    return NotFound(new { status = "error", message = "File not found" });
                }

                Response.Headers.Add("Cache-Control", "public, max-age=31536000");
                Response.Headers.Add("Expires", DateTime.UtcNow.AddYears(1).ToString("R"));

                return File(fileBytes, contentType);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { status = "error", message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Failed to retrieve file", details = ex.Message });
            }
        }

        /// <summary>
        /// رفع صورة جديدة - يحتاج Admin فقط
        /// </summary>
        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { status = "error", message = "No file uploaded" });
                }

                // Validate file size (5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { status = "error", message = "File size must not exceed 5MB" });
                }

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                {
                    return BadRequest(new { status = "error", message = "Only image files (jpg, png, webp) are allowed" });
                }

                var fileUrl = await _fileStorageService.UploadFileAsync(file);

                return Ok(new { status = "success", data = new { fileUrl } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Failed to upload file", details = ex.Message });
            }
        }

        /// <summary>
        /// حذف صورة - يحتاج Admin فقط
        /// </summary>
        [HttpDelete("{fileName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest(new { status = "error", message = "File name is required" });
                }

                await _fileStorageService.DeleteFileAsync(fileName);

                return Ok(new { status = "success", message = "File deleted successfully" });
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { status = "error", message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "Failed to delete file", details = ex.Message });
            }
        }
    }
}