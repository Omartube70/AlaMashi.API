using Application.Dashboard.Dtos;
using MediatR;

namespace Application.Dashboard.Queries
{
    public class GetMonthlySalesQuery : IRequest<List<SalesOverTimeDto>>
    {
        public int Months { get; set; } = 6; // قيمة افتراضية لآخر 6 شهور
    }
}