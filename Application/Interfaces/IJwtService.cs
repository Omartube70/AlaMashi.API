using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        public ClaimsPrincipal ValidateToken(string token);

        public string GeneratePasswordResetToken(User user);
    }
}
