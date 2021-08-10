using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiCatalogo.Pagination;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //private readonly AppDbContext _context;
        //private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;

        //public CategoriasController(AppDbContext context, IConfiguration config,
        //    ILogger <CategoriasController> logger)
        //{
        //    _context = context;
        //    _configuration = config;
        //    _logger = logger;
        //}

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

       
        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                //_logger.LogInformation("============GET api/categorias/produtos ============");

                var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var cat = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id = {id} não foi localizada.");
                }

                var categoria = _mapper.Map<CategoriaDTO>(cat);

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                // O retorno será a categoriaDTO para o usuario

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar criar uma nova categoria.");
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possivel alterar a categoria " +
                        $"com o id={id}");
                }
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

                return Ok($"Categoria com o id = {id} foi atualizada com sucesso");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar alterar a categoria.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id = {id} não foi localizada.");
                }
                

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar excluir a categoria.");
            }

        }


    }
}
