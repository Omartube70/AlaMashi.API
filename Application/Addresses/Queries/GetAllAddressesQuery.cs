using MediatR;
using Application.Addresses.Dtos;
using System.Collections.Generic;

namespace Application.Addresses.Queries
{
    // استعلام لجلب كل العناوين (للأدمن فقط)
    public record GetAllAddressesQuery() : IRequest<IReadOnlyList<AddressDto>>;
}