using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task<User?> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email.ToLower());
        }
        public async Task<bool> IsPhoneTakenAsync(string phone)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token &&
            u.RefreshTokenExpiryTime > System.DateTime.UtcNow);
        }

        public async Task SaveRefreshTokenAsync(int userId, string? refreshToken, System.DateTime? expiryDate)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.SetRefreshToken(refreshToken, expiryDate);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserWithAddressesAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }
    }
}