using System.Security.Claims;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AtendimentoBackend.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly ITokenService _tokenService;

    public AuthController(IUnitOfWork uof, IMapper mapper, IConfiguration configuration, ITokenService tokenService)
    {
        _uof = uof;
        _tokenService = tokenService;
        _mapper = mapper;
        _config = configuration;
    }
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuario = await _uof.UsuarioRepository.GetUsuarioByLogin(login.Email, login.Senha);

        if (usuario == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email)
            };

        var token = _tokenService.GenerateAccessToken(claims, _config);

        return Ok(new { Token = token });
    }
}