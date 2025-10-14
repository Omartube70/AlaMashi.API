using Application.Categories.Dtos;
using Application.Offers.Dtos;
using Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Home.Dtos
{
    public class HomeDataDto
    {
        public List<OfferDto> FeaturedOffers { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public List<CategoryTreeDto> CategoryTree { get; set; } = new();
        public List<ProductDto> NewProducts { get; set; } = new();
        public List<ProductDto> DiscountedProducts { get; set; } = new();
    }
}
