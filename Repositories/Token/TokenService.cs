using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AtendimentoBackend.Repositories;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
    {
        var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var _issuer = _config["Jwt:Issuer"];
        var _audience = _config["Jwt:Audience"];

        var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

        var tokeOptions = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signinCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
}