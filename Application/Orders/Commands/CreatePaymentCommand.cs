using Application.Orders.Dtos;
using Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Commands
{
    public class CreatePaymentCommand : IRequest<PaymentDto>
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentUserId { get; set; }
    }
}
