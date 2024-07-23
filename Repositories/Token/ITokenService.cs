
using System.Security.Claims;

namespace AtendimentoBackend.Repositories;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
}