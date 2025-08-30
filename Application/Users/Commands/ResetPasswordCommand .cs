using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
