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
        [HttpGet("{id}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            return produto;

        }
    }
}
