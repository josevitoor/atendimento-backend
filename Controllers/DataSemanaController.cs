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
public class DataSemanaController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    private static readonly HashSet<string> DiasDaSemana = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "segunda",
        "terça",
        "quarta",
        "quinta",
        "sexta",
        "sábado",
        "domingo"
    };

    public DataSemanaController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetDatasSemanas")]
    public ActionResult<IEnumerable<DataSemanaResponseDTO>> Get()
    {
        var dataSemana = _uof.DataSemanaRepository.GetAll();
        if (dataSemana is null)
        {
            return NotFound();
        }
        var dataSemanaDto = _mapper.Map<IEnumerable<DataSemanaResponseDTO>>(dataSemana);
        return Ok(dataSemanaDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<DataSemanaResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var dataSemana = _uof.DataSemanaRepository.GetDataSemanas(paginationParameters);
        return ObterDataSemanas(dataSemana);
    }

    [HttpGet("{id}", Name = "ObterDataSemana")]
    public ActionResult<DataSemanaResponseDTO> Get(int id)
    {
        var dataSemana = _uof.DataSemanaRepository.Get(c => c.Id == id);
        if (dataSemana is null)
        {
            return NotFound("DataSemana não encontrado...");
        }
        var dataSemanaDto = _mapper.Map<DataSemanaResponseDTO>(dataSemana);
        return Ok(dataSemanaDto);
    }

    [HttpPost]
    public ActionResult<DataSemanaResponseDTO> Post(
        ICollection<DataSemanaRequestDTO> dataSemanaDto,
        [FromQuery] int ServicoId
    )
    {
        if (dataSemanaDto is null)
            return BadRequest();

        if(!dataSemanaDto.All(dto => IsMatchingDay(dto.Dia)))
            throw new ArgumentException("O dia informado não é válido, use o padrão: segunda, terça, quarta...");

        var datasSemanas = _mapper.Map<ICollection<DataSemana>>(dataSemanaDto);

        var novosDatasSemana = _uof.DataSemanaRepository.Create(datasSemanas, ServicoId);
        _uof.Commit();

        var novasDatasSemanasDto = _mapper.Map<ICollection<DataSemanaResponseDTO>>(novosDatasSemana);

        if (novasDatasSemanasDto.Count == 0)
            return NoContent();

        return new CreatedAtRouteResult("GetDatasSemanas", new { }, novasDatasSemanasDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<DataSemanaResponseDTO> Put(int id, DataSemanaRequestDTO dataSemanaDto)
    {
        var dataSemana = _uof.DataSemanaRepository.Get(c => c.Id == id);
        if (dataSemana is null)
        {
            return NotFound("DataSemana não encontrado...");
        }

        _mapper.Map(dataSemanaDto, dataSemana);
        _uof.Commit();

        var dataSemanaAtualizadoDto = _mapper.Map<DataSemanaResponseDTO>(dataSemana);

        return Ok(dataSemanaAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<DataSemanaResponseDTO> Delete(int id)
    {
        var dataSemana = _uof.DataSemanaRepository.Get(p => p.Id == id);
        if (dataSemana is null)
        {
            return NotFound("DataSemana não encontrado...");
        }

        var dataSemanaDeletado = _uof.DataSemanaRepository.Delete(dataSemana);
        _uof.Commit();

        var dataSemanaDeletadoDto = _mapper.Map<DataSemanaResponseDTO>(dataSemanaDeletado);

        return Ok(dataSemanaDeletadoDto);
    }

    private ActionResult<IEnumerable<DataSemanaResponseDTO>> ObterDataSemanas(PagedList<DataSemana> dataSemanas)
    {
        var metadata = new
        {
            dataSemanas.TotalCount,
            dataSemanas.PageSize,
            dataSemanas.CurrentPage,
            dataSemanas.TotalPages,
            dataSemanas.HasNext,
            dataSemanas.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var dataSemanasDto = _mapper.Map<IEnumerable<DataSemanaResponseDTO>>(dataSemanas);
        return Ok(dataSemanasDto);
    }

    private bool IsMatchingDay(string? diaSemana)
    {
        if (string.IsNullOrEmpty(diaSemana))
        {
            return false;
        }

        return DiasDaSemana.Contains(diaSemana.Trim().ToLower());
    }
}