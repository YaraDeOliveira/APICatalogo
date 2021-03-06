using ApiCatalogo.DTOs;
using ApiCatalogo.Filter;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        //Injeção da interface
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }
        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        //Para acessar os dados da tabela
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {

                var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));


                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter os produtos no banco de dados");
            }
        }

        // Para acessar um determinado produto
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"O produto com id = {id} não foi localizado.");
                }

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Erro ao tentar obter os produtos no banco de dados");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDto)
        {

            // Essa verificacao é feita automaticamente a partir da versao 2.1
            // Porém ter que usar [ApiController] 
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try {

                var produto = _mapper.Map<Produto>(produtoDto);
                _uof.ProdutoRepository.Add(produto);
                await _uof.Commit();

                // Tem que exibir o produtoDto e nao o produto
                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult(
                    "ObterProduto",
                    new { id = produtoDto.ProdutoId }, produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao criar um novo produto");
            };
        }
        [HttpPut("{id}")]

        public async Task<ActionResult> Put (int id, [FromBody] ProdutoDTO produtoDto)
        {
            // Essa verificacao é feita automaticamente a partir da versao 2.1
            // Porém ter que usar [ApiController] 
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {

                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest($"O produto com id = {id} não foi localizado.");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();

                return Ok($"Produto com o id = {id} foi atualizado com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao criar um novo produto");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete (int id)
        {
            //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            // Find só funciona se o parametro for a chave primaria
            try
            {

                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"O produto com id = {id} não foi localizado.");
                }

                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                // Tem que exibir o produtoDto e nao o produto
                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return produtoDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar deletar o produto");
            }

        }

    }
}
