using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest
    {
        public int CategoryId { get; }
        public string NewCategoryName { get; }
    }
}
