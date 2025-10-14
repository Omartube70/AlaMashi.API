using Application.Offers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Queries
{
    // Query لطلب قائمة العروض النشطة
    public class GetActiveOffersQuery : IRequest<IEnumerable<OfferDto>>
    {
        // لا تحتاج لمعاملات (Parameters) لأنها تجلب جميع العروض النشطة
    }
}
