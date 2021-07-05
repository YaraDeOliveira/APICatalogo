using ApiCatalogo.Context;
using ApiCatalogo.Filter;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        //Para acessar os dados da tabela
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                return await _context.Produtos.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter os produtos no banco de dados");
            }
        }

        // Para acessar um determinado produto
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto == null)
                { 
                    return NotFound($"O produto com id = {id} não foi localizado.");
                }
                return produto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Erro ao tentar obter os produtos no banco de dados");
            }
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
            try {
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                return new CreatedAtRouteResult(
                    "ObterProduto",
                    new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao criar um novo produto");
            };
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
            try
            {

                if (id != produto.ProdutoId)
                {
                    return BadRequest($"O produto com id = {id} não foi localizado.");
                }
                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok($"Produto com o id = {id} foi atualizado com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao criar um novo produto");
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete (int id)
        {
            //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            // Find só funciona se o parametro for a chave primaria
            try
            {

                var produto = _context.Produtos.Find(id);

                if (produto == null)
                {
                    return NotFound($"O produto com id = {id} não foi localizado.");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar deletar o produto");
            }

        }

    }
}
