using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOfferRepostory
    {
        Task<IEnumerable<Offer>> GetActiveOffersAsync();
        Task<Offer?> GetOfferByIdAsync(int offerId);
        Task AddOfferAsync(Offer offer);
        Task UpdateOfferAsync(Offer offer);
        Task DeleteOfferAsync(int offerId);
    }
}
