using Application.Addresses.Dtos;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Addresses.Commands
{
    public class UpdateAddressPartialCommand : IRequest
    {
            public int AddressId { get; set; }
            public int CurrentUserId { get; set; }
            public JsonPatchDocument<UpdateAddressDto> PatchDoc { get; set; } = null!;
    }
}
