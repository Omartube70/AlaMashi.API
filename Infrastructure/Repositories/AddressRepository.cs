using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetAddressByIdAsync(int addressId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<Address?> GetAddressWithUserAsync(int addressId)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            return await _context.Addresses
                .Include(a => a.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}