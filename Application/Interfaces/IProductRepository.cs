using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int ProductID);
        Task<Product?> GetProductByBarcodeAsync(string Barcode);
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<IReadOnlyList<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
