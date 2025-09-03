using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            // Eager Loading: تحميل الفئة مع المنتج في استعلام واحد
            return await _context.Products.Include(p => p.Category)
                         .FirstOrDefaultAsync(p => p.ProductID == productId);
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category)
                              .AsNoTracking()
                               .ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryID == categoryId) 
                                 .Include(p => p.Category)
                                 .AsNoTracking()
                                 .ToListAsync();
        }
    }
}