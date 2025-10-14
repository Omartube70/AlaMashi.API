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
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public OfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetActiveOffersAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Offers
                .Where(o => o.StartDate <= now && o.EndDate >= now)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Offer>> GetAllOffersAsync()
        {
            return await _context.Offers
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Offer?> GetOfferByIdAsync(int OfferID)
        {
            return await _context.Offers.FirstOrDefaultAsync(p => p.OfferID == OfferID);
        }

        public async Task AddOfferAsync(Offer Offer)
        {
            await _context.Offers.AddAsync(Offer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOfferAsync(Offer Offer)
        {
            _context.Offers.Update(Offer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(Offer Offer)
        {
            if (Offer != null)
            {
                _context.Offers.Remove(Offer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
