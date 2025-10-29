using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class UpdateOrderDeliveryDetailsCommandValidator : AbstractValidator<UpdateOrderDeliveryDetailsCommand>
    {
        public UpdateOrderDeliveryDetailsCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID must be valid.");

            When(x => x.NewDeliveryDate.HasValue, () =>
            {
                RuleFor(x => x.NewDeliveryDate)
                    .Must(date => date >= System.DateTime.UtcNow.Date)
                    .WithMessage("Delivery date cannot be in the past.");
            });

            When(x => x.NewAddressId.HasValue, () =>
            {
                RuleFor(x => x.NewAddressId)
                    .GreaterThan(0).WithMessage("Address ID must be valid.");
            });

            // يجب توفير على الأقل حقل واحد للتحديث
            RuleFor(x => x)
                .Must(cmd => cmd.NewDeliveryDate.HasValue || cmd.NewAddressId.HasValue || !string.IsNullOrEmpty(cmd.NewDeliveryTimeSlot))
                .WithMessage("At least one field must be updated (delivery date, time slot, or address).");
        }
    }
}