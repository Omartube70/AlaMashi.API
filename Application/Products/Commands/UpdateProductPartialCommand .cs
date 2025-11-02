using Application.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Products.Commands
{
    public class UpdateProductPartialCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryID { get; set; }
        public IFormFile? ProductImageFile { get; set; }
    }
}