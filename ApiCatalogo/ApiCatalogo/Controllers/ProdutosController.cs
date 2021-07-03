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
    public class ProdutosController : ControllerBase
    {
        //Injeção de dependencia
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //Para acessar os dados da tabela
        public ActionResult<IEnumerable<Produto>> Get()
        {
           return _context.Produtos.AsNoTracking().ToList();
        }

        // Para acessar um determinado produto
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }
        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {

            // Essa verificacao é feita automaticamente a partir da versao 2.1
            // Porém ter que usar [ApiController] 
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult(
                "ObterProduto", 
                new { id = produto.ProdutoId }, 
                produto );
        }
        [HttpPut("{id}")]

        public ActionResult Put (int id, [FromBody] Produto produto)
        {
            // Essa verificacao é feita automaticamente a partir da versao 2.1
            // Porém ter que usar [ApiController] 
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}


            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete (int id)
        {
            //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            // Find só funciona se o parametro for a chave primaria
            var produto = _context.Produtos.Find(id);
            
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }

    }
}
