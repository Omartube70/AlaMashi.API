using Application.Users.Dtos;
using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateUserPartialCommand : IRequest<Unit> 
    {
        public int TargetUserId { get; set; }
        public JsonPatchDocument<UpdateUserDto> PatchDoc { get; set; }

        // بيانات للتحقق من الصلاحيات
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
