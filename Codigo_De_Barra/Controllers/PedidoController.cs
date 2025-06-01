using Codigo_De_Barra.Database;
using Codigo_De_Barra.DTO;
using Codigo_De_Barra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace Codigo_De_Barra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ProdutosDbContext dbContext;
        public PedidoController(ProdutosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pedido>> GetPedido()
        {
            return Ok(dbContext.Pedidos.Include(p => p.Cliente).Include(p => p.Produtos));
        }

        [HttpGet("{id}")]
         public ActionResult<Pedido> GetPedido(string id) // inclued slide 
        {
            Pedido? pedido = dbContext.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produtos)
                .FirstOrDefault(p => p.Id == id);
                
            if (pedido is null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        public ActionResult<Pedido> CreatePedido(PedidoDTO novoPedidoDTO)
        {
            if (novoPedidoDTO.produtosIds.Length == 0)
            {
                return BadRequest("É necessario enviar a lista de produtos");
            }

            List<Produto> produtos = dbContext
                .Produtos
                .Where(
                    produto => novoPedidoDTO.produtosIds.Contains(produto.Id) 
                ).ToList();

            if (produtos.Count != novoPedidoDTO.produtosIds.Length)
            {
                return BadRequest("Produto não encontrado");
            }

            Cliente? cliente = dbContext
                .Clientes
                .FirstOrDefault(c => c.Id == novoPedidoDTO.cliente);

            if (cliente == null)
            {
                return BadRequest("Usuario inválidado");
            }

            Pedido novoPedido = new Pedido(produtos, cliente, DateTime.Now);



            dbContext.Pedidos.Add(novoPedido);
            dbContext.SaveChanges();



            return CreatedAtAction(nameof(CreatePedido), novoPedido);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePedido(string id, PedidoDTO pedidoAtualizadoDTO)
        {
            Pedido? pedidoEncontrado = dbContext
                .Pedidos
                .FirstOrDefault(p => p.Id == id);
            
            if (pedidoEncontrado is null)
            {
                return NotFound();
            }
            if (pedidoAtualizadoDTO.produtosIds.Length == 0)
            {
                return BadRequest("É necessario enviar a lista de produtos");
            }
            foreach(string produtoId in pedidoAtualizadoDTO.produtosIds)
            {
                Produto? produto = dbContext
                    .Produtos
                    .FirstOrDefault(p => p.Id == produtoId);
                
                if (produto == null)
                {
                    return BadRequest($"Produto {produto.Nome} não encontrado.");
                }
                if (!pedidoEncontrado.Produtos.Contains(produto))
                {
                    pedidoEncontrado.Produtos.Add(produto);
                }
            }
            foreach (string produtoId in pedidoEncontrado.Produtos.Select(p => p.Id).ToList())
            {
                if (!pedidoAtualizadoDTO.produtosIds.Contains(produtoId))
                {
                    Produto? produto = dbContext
                        .Produtos
                        .FirstOrDefault(p => p.Id == produtoId);
                    
                    if (produto != null)
                    {
                        pedidoEncontrado.Produtos.Remove(produto);
                    }
                }
            }

            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePedido(string id)
        {
            Pedido? pedidoEncontrado =
                dbContext
                .Pedidos
                .FirstOrDefault(p => p.Id == id);

            if (pedidoEncontrado == null)
            {
                return NotFound();
            }

            dbContext.Pedidos.Remove(pedidoEncontrado);
            dbContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete("/deleteClientePedido/{idCliente}")]
        public IActionResult DeleteClientePedido(string idCliente)
        {
            Pedido? pedidoEncontrado = dbContext
                .Pedidos
                .FirstOrDefault(p => p.Cliente.Id == idCliente);
            if (pedidoEncontrado == null)
            {
                return NoContent();
            }

            dbContext.Pedidos.Remove(pedidoEncontrado);
            dbContext.SaveChanges();

            return NoContent();

        }
        
    }
}
