using MediatR;
using Application.Addresses.Dtos;
using System.Collections.Generic;


namespace Application.Addresses.Queries
{
    public class GetUserAddressesQuery : IRequest<IEnumerable<AddressDto>>
    {
        public int UserId { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
