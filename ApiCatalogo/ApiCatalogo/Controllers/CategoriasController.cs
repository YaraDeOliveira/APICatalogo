using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            return $"Autor : {autor}";
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categorias.Include(x => x.Produtos).ToList();
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
                return _context.Categorias.AsNoTracking().ToList();
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
                var cat = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

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
                _context.Categorias.Add(categoria);
                _context.SaveChanges();
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
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok($"Categoria com o id = {id} foi atualizada com sucesso");
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar alterar a categoria.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult <Categoria> Delete (int id)
        {
            try
            {
                var cat = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id = {id} não foi localizada.");
                }
                _context.Categorias.Remove(cat);
                _context.SaveChanges();
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
