using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using AtendimentoBackend.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AtendimentoBackend.Controllers;

[Route("[controller]")]
[ApiController]
public class AtendenteController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public AtendenteController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AtendenteResponseDTO>> Get()
    {
        var atendente = _uof.AtendenteRepository.GetAll();
        if (atendente is null)
        {
            return NotFound();
        }
        var atendenteDto = _mapper.Map<IEnumerable<AtendenteResponseDTO>>(atendente);
        return Ok(atendenteDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<AtendenteResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var atendente = _uof.AtendenteRepository.GetAtendentes(paginationParameters);
        return ObterAtendentes(atendente);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<AtendenteResponseDTO>> GetAtendentesFilterPreco
    (
        [FromQuery] AtendenteFiltroDTO atendenteFiltroDTO, 
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var atendentes = _uof.AtendenteRepository.getAtendentesFiltroNome(atendenteFiltroDTO, paginationParameters);
        return ObterAtendentes(atendentes);
    }

    [HttpGet("{id}", Name = "ObterAtendente")]
    public ActionResult<AtendenteResponseDTO> Get(int id)
    {
        var atendente = _uof.AtendenteRepository.Get(c => c.Id == id);
        if (atendente is null)
        {
            return NotFound("Atendente não encontrado...");
        }
        var atendenteDto = _mapper.Map<AtendenteResponseDTO>(atendente);
        return Ok(atendenteDto);
    }

    [HttpPost]
    public ActionResult<AtendenteResponseDTO> Post(AtendenteRequestDTO atendenteDto)
    {
        if (atendenteDto is null)
            return BadRequest();

        var atendente = _mapper.Map<Atendente>(atendenteDto);

        var novoAtendente = _uof.AtendenteRepository.Create(atendente);
        _uof.Commit();

        var novoAtendenteDto = _mapper.Map<AtendenteResponseDTO>(novoAtendente);

        return new CreatedAtRouteResult("ObterAtendente",
            new { id = novoAtendenteDto.Id }, novoAtendenteDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<AtendenteResponseDTO> Put(int id, AtendenteRequestDTO atendenteDto)
    {
        var atendente = _uof.AtendenteRepository.Get(c => c.Id == id);
        if (atendente is null)
        {
            return NotFound("Atendente não encontrado...");
        }

        _mapper.Map(atendenteDto, atendente);
        _uof.Commit();

        var atendenteAtualizadoDto = _mapper.Map<AtendenteResponseDTO>(atendente);

        return Ok(atendenteAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<AtendenteResponseDTO> Delete(int id)
    {
        var atendente = _uof.AtendenteRepository.Get(p => p.Id == id);
        if (atendente is null)
        {
            return NotFound("Atendente não encontrado...");
        }

        var atendenteDeletado = _uof.AtendenteRepository.Delete(atendente);
        _uof.Commit();

        var atendenteDeletadoDto = _mapper.Map<AtendenteResponseDTO>(atendenteDeletado);

        return Ok(atendenteDeletadoDto);
    }

    private ActionResult<IEnumerable<AtendenteResponseDTO>> ObterAtendentes(PagedList<Atendente> atendentes)
    {
        var metadata = new
        {
            atendentes.TotalCount,
            atendentes.PageSize,
            atendentes.CurrentPage,
            atendentes.TotalPages,
            atendentes.HasNext,
            atendentes.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var atendentesDto = _mapper.Map<IEnumerable<AtendenteResponseDTO>>(atendentes);
        return Ok(atendentesDto);
    }
}