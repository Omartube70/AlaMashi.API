using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
        public interface IUserRepository
        {
            Task<User?> FindByEmailAsync(string email);

            Task<User?> FindByUserIdAsync(int UserID);
         
            Task DeleteUserAsync(int UserID);
        
            Task SaveAsync(User user);
        }
}

