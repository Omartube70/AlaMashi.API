
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Addresses.Dtos
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? AddressDetails { get; set; }
        public string AddressType { get; set; }
        public int UserId { get; init; }

    }
}
