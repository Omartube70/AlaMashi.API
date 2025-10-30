using MediatR;
using Application.Addresses.Dtos;
namespace Application.Addresses.Queries
{
    public class GetAddressByIdQuery : IRequest<AddressDto>
    {
        public int AddressId { get; set; }
        public int CurrentUserId { get; set; }

        public bool IsAdmin { get; set; }
    }
}
