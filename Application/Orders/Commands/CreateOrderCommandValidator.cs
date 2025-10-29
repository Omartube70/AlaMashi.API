using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CurrentUserId)
                .GreaterThan(0).WithMessage("User ID must be valid.");

            RuleFor(x => x.AddressId)
                .GreaterThan(0).WithMessage("Address ID must be valid.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("Order items cannot be empty.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .GreaterThan(0).WithMessage("Product ID must be valid.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            });

            When(x => x.DeliveryDate.HasValue, () =>
            {
                RuleFor(x => x.DeliveryDate.Value)
                    .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                    .WithMessage("Delivery date cannot be in the past.");
            });
        }
    }

}
