using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressByIdAsync(int addressId);
        Task<Address?> GetAddressWithUserAsync(int addressId);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);
        Task<IReadOnlyList<Address>> GetAllAddressesAsync();
        Task<IReadOnlyList<Address>> GetAddressesByUserIdAsync(int userId);
    }
}