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
public class DataServicoController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public DataServicoController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<DataServicoResponseDTO>> Get()
    {
        var dataServico = _uof.DataServicoRepository.GetAllWithRelations();
        if (dataServico is null)
        {
            return NotFound();
        }
        var dataServicoDto = _mapper.Map<IEnumerable<DataServicoResponseDTO>>(dataServico);
        return Ok(dataServicoDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<DataServicoResponseDTO>> Get([FromQuery] PaginationParameters paginationParameters)
    {
        var dataServico = _uof.DataServicoRepository.GetDataServicos(paginationParameters);
        return ObterDataServicos(dataServico);
    }

    [HttpGet("filter/{id}/pagination")]
    public ActionResult<IEnumerable<DataServicoResponseDTO>> GetDataServicosFilterPreco
    (
        int id,
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var dataServicos = _uof.DataServicoRepository.getDataServicosFiltroId(id, paginationParameters);
        return ObterDataServicos(dataServicos);
    }

    [HttpGet("{id}", Name = "ObterDataServico")]
    public ActionResult<DataServicoResponseDTO> Get(int id)
    {
        var dataServico = _uof.DataServicoRepository.GetWithRelations(id);
        if (dataServico is null)
        {
            return NotFound("DataServico não encontrado...");
        }
        var dataServicoDto = _mapper.Map<DataServicoResponseDTO>(dataServico);
        return Ok(dataServicoDto);
    }

    [HttpPost]
    public ActionResult<DataServicoResponseDTO> Post(DataServicoRequestDTO dataServicoDto)
    {
        if (dataServicoDto is null)
            return BadRequest();

        var dataServico = _mapper.Map<DataServico>(dataServicoDto);

        var novoDataServico = _uof.DataServicoRepository.Create(dataServico);
        _uof.Commit();

        var novoDataServicoDto = _mapper.Map<DataServicoResponseDTO>(novoDataServico);

        return new CreatedAtRouteResult("ObterDataServico",
            new { id = novoDataServicoDto.Id }, novoDataServicoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<DataServicoResponseDTO> Put(int id, DataServicoRequestDTO dataServicoDto)
    {
        var dataServico = _uof.DataServicoRepository.Get(c => c.Id == id);
        if (dataServico is null)
        {
            return NotFound("DataServico não encontrado...");
        }

        _mapper.Map(dataServicoDto, dataServico);
        _uof.Commit();

        var dataServicoAtualizadoDto = _mapper.Map<DataServicoResponseDTO>(dataServico);

        return Ok(dataServicoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<DataServicoResponseDTO> Delete(int id)
    {
        var dataServico = _uof.DataServicoRepository.Get(p => p.Id == id);
        if (dataServico is null)
        {
            return NotFound("DataServico não encontrado...");
        }

        var dataServicoDeletado = _uof.DataServicoRepository.Delete(dataServico);
        _uof.Commit();

        var dataServicoDeletadoDto = _mapper.Map<DataServicoResponseDTO>(dataServicoDeletado);

        return Ok(dataServicoDeletadoDto);
    }

    private ActionResult<IEnumerable<DataServicoResponseDTO>> ObterDataServicos(PagedList<DataServico> dataServicos)
    {
        var metadata = new
        {
            dataServicos.TotalCount,
            dataServicos.PageSize,
            dataServicos.CurrentPage,
            dataServicos.TotalPages,
            dataServicos.HasNext,
            dataServicos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var dataServicosDto = _mapper.Map<IEnumerable<DataServicoResponseDTO>>(dataServicos);
        return Ok(dataServicosDto);
    }
}