using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; }
    }
}
