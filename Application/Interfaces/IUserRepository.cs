using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefreshTokenAsync(string token);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> IsEmailTakenAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task SaveRefreshTokenAsync(int userId, string? refreshToken, DateTime expiryDate);

        Task<User?> GetUserWithAddressesAsync(int userId);
    }
}
