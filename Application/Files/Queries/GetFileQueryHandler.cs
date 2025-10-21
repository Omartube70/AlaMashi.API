using Application.Files.Queries;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Files.Handlers
{
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, (byte[] FileBytes, string ContentType)>
    {
        private readonly IFileStorageService _fileStorageService;

        public GetFileQueryHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<(byte[] FileBytes, string ContentType)> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            return await _fileStorageService.GetFileAsync(request.FileName);
        }
    }
}
