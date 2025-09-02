using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Addresses.Commands
{
    public class DeleteAddressCommand : IRequest
    {
        public int AddressId { get; set; }

        public int CurrentUserId { get; set; }
    }
}
