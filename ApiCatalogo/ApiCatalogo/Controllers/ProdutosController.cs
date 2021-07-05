using ApiCatalogo.Filter;
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
    public class ProdutosController : ControllerBase
    {
        //Injeção da interface
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }
        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        //Para acessar os dados da tabela
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return  _uof.ProdutoRepository.Get().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter os produtos no banco de dados");
            }
        }

        // Para acessar um determinado produto
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();
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
                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();
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

                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"O produto com id = {id} não foi localizado.");
                }

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();
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
