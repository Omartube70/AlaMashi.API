using Application.Offers.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class CreateOfferValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOfferValidator()
        {
            RuleFor(x => x.Title)
      .NotEmpty().WithMessage("Title is required.")
      .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("Offer Description must not exceed 500 characters.");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Discount percentage must be between 0 and 100.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");
        }
    }
}
