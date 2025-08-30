using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using System.ComponentModel.DataAnnotations;


namespace Application.Addresses.Dtos
{
    public class UpdateAddressDto
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
