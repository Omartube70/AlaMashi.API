using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OfferRepostory : IOfferRepostory
    {
        private readonly ApplicationDbContext _context;

        public OfferRepostory(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- تنفيذ عمليات القراءة (Queries) ---

        public async Task<IEnumerable<Offer>> GetActiveOffersAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Offers
                .Where(o => o.StartDate <= now && o.EndDate >= now)
                .AsNoTracking() 
                .ToListAsync();
        }

        public async Task<Offer?> GetOfferByIdAsync(int offerId)
        {
            return await _context.Offers
                .FirstOrDefaultAsync(o => o.OfferID == offerId);
        }

        // --- تنفيذ عمليات الكتابة (Commands) ---

        public async Task AddOfferAsync(Offer offer)
        {
            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOfferAsync(Offer offer)
        {
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(int offerId)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
