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
[Authorize]
public class ServicoController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ServicoController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ServicoResponseDTO>> Get()
    {
        var servico = _uof.ServicoRepository.GetAllWithRelations();
        if (servico is null)
        {
            return NotFound();
        }
        var servicoDto = _mapper.Map<IEnumerable<ServicoResponseDTO>>(servico);
        return Ok(servicoDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ServicoResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var servico = _uof.ServicoRepository.GetServicos(paginationParameters);
        return ObterServicos(servico);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<ServicoResponseDTO>> GetServicosFilterPreco
    (
        [FromQuery] ServicoFiltroDTO servicoFiltroDTO,
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var servicos = _uof.ServicoRepository.getServicosFiltroNome(servicoFiltroDTO, paginationParameters);
        return ObterServicos(servicos);
    }

    [HttpGet("{id}", Name = "ObterServico")]
    public ActionResult<ServicoResponseDTO> Get(int id)
    {
        var servico = _uof.ServicoRepository.GetWithRelations(id);
        if (servico is null)
        {
            return NotFound("Servico não encontrado...");
        }
        var servicoDto = _mapper.Map<ServicoResponseDTO>(servico);
        return Ok(servicoDto);
    }

    [HttpPost]
    public ActionResult<ServicoResponseDTO> Post(ServicoRequestDTO servicoDto)
    {
        if (servicoDto is null)
            return BadRequest();

        var servico = _mapper.Map<Servico>(servicoDto);

        var novoServico = _uof.ServicoRepository.Create(servico);
        _uof.Commit();

        var novoServicoDto = _mapper.Map<ServicoResponseDTO>(novoServico);

        return new CreatedAtRouteResult("ObterServico",
            new { id = novoServicoDto.Id }, novoServicoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ServicoResponseDTO> Put(int id, ServicoRequestDTO servicoDto)
    {
        var servico = _uof.ServicoRepository.Get(c => c.Id == id);
        if (servico is null)
        {
            return NotFound("Servico não encontrado...");
        }

        _mapper.Map(servicoDto, servico);
        _uof.Commit();

        var servicoAtualizadoDto = _mapper.Map<ServicoResponseDTO>(servico);

        return Ok(servicoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ServicoResponseDTO> Delete(int id)
    {
        var servico = _uof.ServicoRepository.Get(p => p.Id == id);
        if (servico is null)
        {
            return NotFound("Servico não encontrado...");
        }

        var servicoDeletado = _uof.ServicoRepository.Delete(servico);
        _uof.Commit();

        var servicoDeletadoDto = _mapper.Map<ServicoResponseDTO>(servicoDeletado);

        return Ok(servicoDeletadoDto);
    }

    private ActionResult<IEnumerable<ServicoResponseDTO>> ObterServicos(PagedList<Servico> servicos)
    {
        var metadata = new
        {
            servicos.TotalCount,
            servicos.PageSize,
            servicos.CurrentPage,
            servicos.TotalPages,
            servicos.HasNext,
            servicos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var servicosDto = _mapper.Map<IEnumerable<ServicoResponseDTO>>(servicos);
        return Ok(servicosDto);
    }
}