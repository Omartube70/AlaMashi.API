using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using System.ComponentModel.DataAnnotations;
using Application.Orders.Dtos;

namespace Application.Orders.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryTimeSlot { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AddressId { get; set; }
        public string AddressDetails { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
        public decimal TotalPaid { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
