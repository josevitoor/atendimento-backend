using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using AtendimentoBackend.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AtendimentoBackend.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public UsuarioController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UsuarioResponseDTO>> Get()
    {
        var usuarios = _uof.UsuarioRepository.GetAll();
        if (usuarios is null)
        {
            return NotFound();
        }
        var usuariosDTO = _mapper.Map<IEnumerable<UsuarioResponseDTO>>(usuarios);
        return Ok(usuariosDTO);
    }

    [HttpGet("{id:int}")]
    public ActionResult<UsuarioResponseDTO> GetById(int id)
    {
        var usuario = _uof.UsuarioRepository.Get(p => p.Id == id);

        if (usuario is null)
            return NotFound("Usuário não encontrado...");

        var usuarioDto = _mapper.Map<UsuarioResponseDTO>(usuario);

        return Ok(usuarioDto);
    }

    [HttpPost]
    public ActionResult<UsuarioResponseDTO> Post(UsuarioRequestDTO usuarioPostDTO)
    {
        if (usuarioPostDTO is null)
            return BadRequest();

        var usuario = _mapper.Map<Usuario>(usuarioPostDTO);

        var novoUsuario = _uof.UsuarioRepository.Create(usuario);
        _uof.Commit();

        var novoUsuarioDTO = _mapper.Map<UsuarioResponseDTO>(novoUsuario);

        return Ok(novoUsuarioDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<UsuarioResponseDTO> Put(int id, UsuarioRequestDTO usuarioDTO)
    {
        var usuario = _mapper.Map<Usuario>(usuarioDTO);
        usuario.Id = id;

        var usuarioAtualizado = _uof.UsuarioRepository.Update(usuario);
        _uof.Commit();

        var usuarioAtualizadoDTO = _mapper.Map<UsuarioResponseDTO>(usuarioAtualizado);

        return Ok(usuarioAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<UsuarioResponseDTO> Delete(int id)
    {
        var usuario = _uof.UsuarioRepository.Get(p => p.Id == id);
        if (usuario is null)
        {
            return NotFound("Usuário não encontrado...");
        }

        var usuarioDeletado = _uof.UsuarioRepository.Delete(usuario);
        _uof.Commit();

        var usuarioDeletadoDTO = _mapper.Map<UsuarioResponseDTO>(usuarioDeletado);

        return Ok(usuarioDeletadoDTO);
    }
}