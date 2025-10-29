using Application.Dashboard.Dtos;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
            return await _context.Products.Include(p => p.Category)
                         .FirstOrDefaultAsync(p => p.ProductID == productId);
        }

        public async Task<Product?> GetProductByBarcodeAsync(string Barcode)
        {
            return await _context.Products.Include(p => p.Category)
                         .FirstOrDefaultAsync(p => p.Barcode == Barcode);
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

        //القارير
        public async Task<List<TopProductDto>> GetTopSellingProductsAsync(DateTime? StartDate, DateTime? EndDate, int TopCount , CancellationToken cancellationToken)
        {
            var query = _context.OrderDetails.Include(od => od.Product).Include(od => od.Order)
                .Where(od => od.Order.Status == OrderStatus.Delivered);

            if (StartDate.HasValue)
                query = query.Where(od => od.Order.OrderDate >= StartDate.Value);

            if (EndDate.HasValue)
                query = query.Where(od => od.Order.OrderDate <= EndDate.Value);

           return await query
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantitySold = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Subtotal)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(TopCount)
                .ToListAsync(cancellationToken);
        }
    }
}