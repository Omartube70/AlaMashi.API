using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Dtos
{
    public class ProductDetailsDto
    {
        public int ProductID { get; init; }
        public string ProductName { get; init; }
        public string? ProductDescription { get; init; }
        public string? Barcode { get; init; }
        public decimal Price { get; init; }
        public int QuantityInStock { get; init; }
        public string MainImageURL { get; init; }
        public int CategoryId { get; init; }
        public string CategoryName { get; init; }
        public List<ProductOrderDto>? Orders { get; init; }
    }
}
