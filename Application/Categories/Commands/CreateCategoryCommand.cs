using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Categories.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string CategoryName { get; set; }
        public IFormFile CategoryImageFile { get; set; }
        public int? ParentId { get; set; }
    }
}
