using MediatR;

namespace Application.Files.Queries
{
    public class GetFileQuery : IRequest<(byte[] FileBytes, string ContentType)>
    {
        public string FileName { get; set; }
    }
}
