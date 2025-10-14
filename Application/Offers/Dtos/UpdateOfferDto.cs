﻿namespace Application.Offers.Dtos
{
    public class UpdateOfferDto
    {
        public int OfferID { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsActive { get; set; }
    }
}