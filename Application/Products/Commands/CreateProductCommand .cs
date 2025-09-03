using Application.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string ProductName { get; set;}
        public string Barcode { get; set;}
        public string? ProductDescription { get; set;}
        public decimal Price { get; set;}
        public int QuantityInStock { get; set;}
        public int CategoryID { get; set;}

        public IFormFile ProductImageFile { get; set; }
    }
}
