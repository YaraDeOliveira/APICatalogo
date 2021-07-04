using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(x => x.Produtos).ToList();
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var cat = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

            if (cat == null)
            {
                return NotFound();
            }
            return cat;
               
        }

        [HttpPost]

        public ActionResult Post([FromBody] Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult <Categoria> Delete (int id)
        {
            var cat = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (cat == null)
            {
                return NotFound();
            }
            _context.Categorias.Remove(cat);
            _context.SaveChanges();
            return cat;
        }


    }
}
