using Domain.Common;
using MediatR;

namespace Application.Addresses.Commands
{
    public class UpdateAddressCommand : IRequest
    {
        public int CurrentUserId { get; set; }
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? AddressDetails { get; set; }
        public AddressType AddressType { get; set; }
    }
}
