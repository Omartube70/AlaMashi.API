using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOfferRepository
    {
        Task<IEnumerable<Offer>> GetActiveOffersAsync();
        Task<IEnumerable<Offer>> GetAllOffersAsync();
        Task<Offer?> GetOfferByIdAsync(int OfferID);
        Task AddOfferAsync(Offer Offer);
        Task UpdateOfferAsync(Offer Offer);
        Task DeleteOfferAsync(Offer Offer);
    }
}
