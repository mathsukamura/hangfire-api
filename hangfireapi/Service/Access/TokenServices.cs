using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apiemail.Model;
using Microsoft.IdentityModel.Tokens;

namespace apiemail.Service.Access;

public interface ITokenService
{
    string GerarToken(User usuarios);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GerarToken(User usuarios)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var keyJwt = _config.GetValue<dynamic>("Jwt:Key");
        //var jwtKey = Convert.ToBase64String(keyBytes);
        var key = Encoding.ASCII.GetBytes(keyJwt);

        var issuer = _config.GetValue<string>("Jwt:Issuer");
        var audience = _config.GetValue<string>("Jwt:Audience");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sid, usuarios.Id.ToString()),    
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }
            ),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}