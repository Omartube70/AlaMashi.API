using Application.Orders.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>
    {
    }
}
