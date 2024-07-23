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
public class AgendamentoController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public AgendamentoController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AgendamentoResponseDTO>> Get()
    {
        var agendamento = _uof.AgendamentoRepository.GetAll();
        if (agendamento is null)
        {
            return NotFound();
        }
        var agendamentoDto = _mapper.Map<IEnumerable<AgendamentoResponseDTO>>(agendamento);
        return Ok(agendamentoDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<AgendamentoResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var agendamento = _uof.AgendamentoRepository.GetAgendamentos(paginationParameters);
        return ObterAgendamentos(agendamento);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<AgendamentoResponseDTO>> GetAgendamentosFilterPreco
    (
        [FromQuery] AgendamentoFiltroDTO agendamentoFiltroDTO, 
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var agendamentos = _uof.AgendamentoRepository.getAgendamentosFiltroNome(agendamentoFiltroDTO, paginationParameters);
        return ObterAgendamentos(agendamentos);
    }

    [HttpGet("{id}", Name = "ObterAgendamento")]
    public ActionResult<AgendamentoResponseDTO> Get(int id)
    {
        var agendamento = _uof.AgendamentoRepository.Get(c => c.Id == id);
        if (agendamento is null)
        {
            return NotFound("Agendamento não encontrado...");
        }
        var agendamentoDto = _mapper.Map<AgendamentoResponseDTO>(agendamento);
        return Ok(agendamentoDto);
    }

    [HttpPost]
    public ActionResult<AgendamentoResponseDTO> Post(AgendamentoRequestDTO agendamentoDto)
    {
        if (agendamentoDto is null)
            return BadRequest();

        var agendamento = _mapper.Map<Agendamento>(agendamentoDto);

        var novoAgendamento = _uof.AgendamentoRepository.CreateAgendamento(agendamento);
        _uof.Commit();

        var novoAgendamentoDto = _mapper.Map<AgendamentoResponseDTO>(novoAgendamento);

        return new CreatedAtRouteResult("ObterAgendamento",
            new { id = novoAgendamentoDto.Id }, novoAgendamentoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<AgendamentoResponseDTO> Put(int id, AgendamentoRequestDTO agendamentoDto)
    {
        var agendamento = _uof.AgendamentoRepository.Get(c => c.Id == id);
        if (agendamento is null)
        {
            return NotFound("Agendamento não encontrado...");
        }

        _mapper.Map(agendamentoDto, agendamento);
        _uof.Commit();

        var agendamentoAtualizadoDto = _mapper.Map<AgendamentoResponseDTO>(agendamento);

        return Ok(agendamentoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<AgendamentoResponseDTO> Delete(int id)
    {
        var agendamento = _uof.AgendamentoRepository.Get(p => p.Id == id);
        if (agendamento is null)
        {
            return NotFound("Agendamento não encontrado...");
        }

        var agendamentoDeletado = _uof.AgendamentoRepository.Delete(agendamento);
        _uof.Commit();

        var agendamentoDeletadoDto = _mapper.Map<AgendamentoResponseDTO>(agendamentoDeletado);

        return Ok(agendamentoDeletadoDto);
    }

    private ActionResult<IEnumerable<AgendamentoResponseDTO>> ObterAgendamentos(PagedList<Agendamento> agendamentos)
    {
        var metadata = new
        {
            agendamentos.TotalCount,
            agendamentos.PageSize,
            agendamentos.CurrentPage,
            agendamentos.TotalPages,
            agendamentos.HasNext,
            agendamentos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoResponseDTO>>(agendamentos);
        return Ok(agendamentosDto);
    }
}