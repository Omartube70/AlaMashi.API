using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
            private readonly int _tokenByteLength = 64;
            private readonly int _tokenLifetimeInDays = 60;

            public (string token, DateTime expiryTime) Generate()
            {
                var randomNumber = new byte[_tokenByteLength];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);

                string token = Convert.ToBase64String(randomNumber);
                DateTime expiryTime = DateTime.UtcNow.AddDays(_tokenLifetimeInDays);

                return (token, expiryTime);
            }
    }
    
}
