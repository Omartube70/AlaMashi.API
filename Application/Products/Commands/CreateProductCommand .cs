using Application.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string ProductName { get; }
        public string Barcode { get; }
        public string? ProductDescription { get; }
        public decimal Price { get; }
        public int QuantityInStock { get; }
        public string MainImageURL { get; }
        public int CategoryID { get; }
        public int? BrandID { get; }

    }
}
