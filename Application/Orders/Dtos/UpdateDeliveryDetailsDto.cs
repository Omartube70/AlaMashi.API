using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using System.ComponentModel.DataAnnotations;


namespace Application.Orders.Dtos
{
    public class UpdateDeliveryDetailsDto
    {
        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string DeliveryTimeSlot { get; set; }
    }
}
