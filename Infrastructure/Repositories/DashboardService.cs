using Application.Dashboard.Dtos;
using Application.Interfaces; 
using Infrastructure.Data; 
using Microsoft.EntityFrameworkCore; 
using System.Linq;

namespace Infrastructure.Repositories
{ 
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
            {
            // Fetch counts one after the other (Solves the problem)
            int totalUsers = await _context.Users.CountAsync(cancellationToken);
            int totalProducts = await _context.Products.CountAsync(cancellationToken);
            int totalCategories = await _context.Categories.CountAsync(cancellationToken);
            int totalActiveOffers = await _context.Offers
                                          .CountAsync(o => o.EndDate >= DateTime.UtcNow && o.StartDate <= DateTime.UtcNow, cancellationToken);

            // Create the DTO with the results
            var summary = new DashboardSummaryDto
            {
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalActiveOffers = totalActiveOffers
                // Add other counts here if needed
            };

            return summary;
        }
        }
    }
