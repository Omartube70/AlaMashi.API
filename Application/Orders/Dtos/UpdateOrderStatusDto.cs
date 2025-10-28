using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class CreateAddressDto
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        public string? AddressDetails { get; set; }

        [Required]
        public AddressType AddressType { get; set; }
    }
}
