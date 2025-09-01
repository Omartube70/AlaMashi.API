using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Dtos
{
    public record ProductDto
    {
        public int ProductID { get; init; }
        public string ProductName { get; init; }
        public string? ProductDescription { get; init; }
        public decimal Price { get; init; }
        public int QuantityInStock { get; init; }
        public string MainImageURL { get; init; }

        // تم تبسيط البيانات المرتبطة
        public string CategoryName { get; init; }
    }
}
