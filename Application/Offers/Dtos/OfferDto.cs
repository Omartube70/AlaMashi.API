﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Dtos
{
    public class OfferDto
    {
        public int OfferID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<OfferProductDto>? Products { get; set; }
    }
}
