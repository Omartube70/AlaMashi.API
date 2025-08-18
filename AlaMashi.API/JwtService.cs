using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlaMashi.BLL;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration; // إضافة هذه المكتبة

public class JwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _Audience;
    // استخدام IConfiguration مباشرة في الـ constructor
    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured.");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is not configured.");
        _Audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is not configured.");
    }

    public string GenerateToken(int userId, string username, UserBLL.enPermissions permissions, int expireMinutes = 60)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username), // استخدام ClaimTypes.Name أفضل للتعريف
                new Claim(ClaimTypes.Role, permissions.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = _issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = false, // يمكن تغييرها إلى true إذا كان لديك جمهور محدد
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception ex)
        {
            // TODO: تسجيل الخطأ هنا (log the exception)
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }


    public string GenerateResetToken(int userId, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(1); // صلاحية الرمز ساعة واحدة

        var token = new JwtSecurityToken(
            _issuer,
            _Audience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}

