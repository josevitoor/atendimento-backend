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
public class PacienteController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public PacienteController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PacienteResponseDTO>> Get()
    {
        var paciente = _uof.PacienteRepository.GetAll();
        if (paciente is null)
        {
            return NotFound();
        }
        var pacienteDto = _mapper.Map<IEnumerable<PacienteResponseDTO>>(paciente);
        return Ok(pacienteDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<PacienteResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var paciente = _uof.PacienteRepository.GetPacientes(paginationParameters);
        return ObterPacientes(paciente);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<PacienteResponseDTO>> GetPacientesFilterPreco
    (
        [FromQuery] PacienteFiltroDTO pacienteFiltroDTO,
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var pacientes = _uof.PacienteRepository.getPacientesFiltroNome(pacienteFiltroDTO, paginationParameters);
        return ObterPacientes(pacientes);
    }

    [HttpGet("{id}", Name = "ObterPaciente")]
    public ActionResult<PacienteResponseDTO> Get(int id)
    {
        var paciente = _uof.PacienteRepository.Get(c => c.Id == id);
        if (paciente is null)
        {
            return NotFound("Paciente não encontrado...");
        }
        var pacienteDto = _mapper.Map<PacienteResponseDTO>(paciente);
        return Ok(pacienteDto);
    }

    [HttpPost]
    public ActionResult<PacienteResponseDTO> Post(PacienteRequestDTO pacienteDto)
    {
        if (pacienteDto is null)
            return BadRequest();

        var paciente = _mapper.Map<Paciente>(pacienteDto);

        var novoPaciente = _uof.PacienteRepository.Create(paciente);
        _uof.Commit();

        var novoPacienteDto = _mapper.Map<PacienteResponseDTO>(novoPaciente);

        return new CreatedAtRouteResult("ObterPaciente",
            new { id = novoPacienteDto.Id }, novoPacienteDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<PacienteResponseDTO> Put(int id, PacienteRequestDTO pacienteDto)
    {
        var paciente = _uof.PacienteRepository.Get(c => c.Id == id);
        if (paciente is null)
        {
            return NotFound("Paciente não encontrado...");
        }

        _mapper.Map(pacienteDto, paciente);
        _uof.Commit();

        var pacienteAtualizadoDto = _mapper.Map<PacienteResponseDTO>(paciente);

        return Ok(pacienteAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<PacienteResponseDTO> Delete(int id)
    {
        var paciente = _uof.PacienteRepository.Get(p => p.Id == id);
        if (paciente is null)
        {
            return NotFound("Paciente não encontrado...");
        }

        var pacienteDeletado = _uof.PacienteRepository.Delete(paciente);
        _uof.Commit();

        var pacienteDeletadoDto = _mapper.Map<PacienteResponseDTO>(pacienteDeletado);

        return Ok(pacienteDeletadoDto);
    }

    private ActionResult<IEnumerable<PacienteResponseDTO>> ObterPacientes(PagedList<Paciente> pacientes)
    {
        var metadata = new
        {
            pacientes.TotalCount,
            pacientes.PageSize,
            pacientes.CurrentPage,
            pacientes.TotalPages,
            pacientes.HasNext,
            pacientes.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var pacientesDto = _mapper.Map<IEnumerable<PacienteResponseDTO>>(pacientes);
        return Ok(pacientesDto);
    }
}