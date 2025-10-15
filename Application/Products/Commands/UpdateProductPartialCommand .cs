using Application.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UpdateProductPartialCommand : IRequest
    {
        public int ProductId { get; set; }
        public JsonPatchDocument<UpdateProductDto> PatchDoc { get; set; } = null!;
    }
}
