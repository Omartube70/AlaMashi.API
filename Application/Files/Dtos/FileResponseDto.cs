namespace Application.Files.Dtos
{
    public class FileResponseDto
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
