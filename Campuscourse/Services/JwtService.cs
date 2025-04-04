using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Campuscourse.Models;
using Microsoft.IdentityModel.Tokens;

namespace Campuscourse.Services;

public class JwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _tokenLifetimeInHours;

    public JwtService(IConfiguration config)
    {
        _secret = config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured");
        _issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
        _audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");
        _tokenLifetimeInHours = int.Parse(config["Jwt:TokenLifetimeInHours"] ?? "1");
    }

    public string GenerateToken(UserModel userModel)
    {
        if (userModel == null) throw new ArgumentNullException(nameof(userModel));

        var claims = new List<Claim>
        {
            new Claim("Id", userModel.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userModel.FullName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, userModel.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (userModel.Role != null)
        {
            if (userModel.Role.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            if (userModel.Role.IsTeacher) claims.Add(new Claim(ClaimTypes.Role, "Teacher"));
            if (userModel.Role.IsStudent) claims.Add(new Claim(ClaimTypes.Role, "Student"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(_tokenLifetimeInHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
