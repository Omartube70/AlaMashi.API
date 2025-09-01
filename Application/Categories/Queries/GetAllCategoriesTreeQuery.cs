using Application.Categories.Dtos;
using MediatR;


namespace Application.Categories.Queries
{
    public class GetAllCategoriesTreeQuery : IRequest<IReadOnlyList<CategoryTreeDto>>
    {
    }
}
