using Application.Files.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AlaMashi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{fileName}")]
        [Authorize]
        public async Task<IActionResult> GetFile(string fileName)
        {
             var (fileBytes, contentType) = await _mediator.Send(new GetFileQuery { FileName = fileName });
             return File(fileBytes, contentType);
        }
    }
}
