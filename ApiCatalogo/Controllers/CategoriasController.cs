using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public CategoriasController(IUnitOfWork context)
        {
            _uof = context;
        }

        //[HttpGet("autor")]
        //public string GetAutor()
        //{
        //    var autor = _configuration["autor"];
        //    return $"Autor : {autor}";
        //}

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                //_logger.LogInformation("============GET api/categorias/produtos ============");
                return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }

        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _uof.CategoriaRepository.Get().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var cat = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id = {id} não foi localizada.");
                }
                return cat;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados.");
            }
        }

        [HttpPost]

        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();
                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar criar uma nova categoria.");
            }

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"Não foi possivel alterar a categoria " +
                        $"com o id={id}");
                }
                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
                return Ok($"Categoria com o id = {id} foi atualizada com sucesso");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar alterar a categoria.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var cat = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id = {id} não foi localizada.");
                }
                _uof.CategoriaRepository.Delete(cat);
                _uof.Commit();
                return cat;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar excluir a categoria.");
            }

        }


    }
}
