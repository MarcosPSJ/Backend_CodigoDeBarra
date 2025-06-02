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
            return Ok(dbContext.Pedidos.Include(p => p.Cliente).Include(p => p.Produtos).OrderByDescending(p => p.DataPedido));
        }

        [HttpGet("{id}")]
         public ActionResult<Pedido> GetPedido(string id)
        {
            Pedido? pedido = dbContext.Pedidos
                .Include(p => p.Produtos)
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.DataPedido)
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
                .FirstOrDefault(c => c.Id == novoPedidoDTO.clienteId);

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
                .Include(p => p.Produtos)
                .FirstOrDefault(p => p.Id == id);
            
            if (pedidoEncontrado is null)
            {
                return NotFound("Pedido não encontrado"); // NotFound = Cliente mandou errado
            }
            if (pedidoAtualizadoDTO.produtosIds.Length == 0)
            {
                return BadRequest("É necessario enviar a lista de produtos"); // BadResquest = Cliente pediu algo que não existe
            }

            List<Produto> produtosVerificados = dbContext
                .Produtos
                .Where(produto => pedidoAtualizadoDTO.produtosIds.Contains(produto.Id))
                .ToList();

            if (produtosVerificados.Count != pedidoAtualizadoDTO.produtosIds.Length)
            {
                return BadRequest("Produto não encontrado");
            }

            pedidoEncontrado.Produtos.Clear(); // Limpa os produtos do pedido antes de adicionar os novos
            foreach (Produto produtoId in produtosVerificados)
            {
                if (!pedidoEncontrado.Produtos.Contains(produtoId))
                {
                    pedidoEncontrado.Produtos.Add(produtoId);
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
            Cliente? clienteEncontrado = dbContext
                .Clientes
                .FirstOrDefault(c => c.Id == idCliente);
            if (clienteEncontrado == null)
            {
                return BadRequest("Usuário não encontrado");
            }
            
            Pedido? pedidoEncontrado = dbContext
                .Pedidos
                .Include(p => p.Produtos)
                .OrderByDescending(p => p.DataPedido)
                .FirstOrDefault(p => p.Cliente.Id.Equals(idCliente));
            if (pedidoEncontrado == null)
            {
                return NoContent();
            }
            
            pedidoEncontrado.Produtos.Clear(); // Limpa os produtos do pedido e evitar erros de chave estrangeira
            dbContext.SaveChanges();

            dbContext.Pedidos.Remove(pedidoEncontrado);
            dbContext.SaveChanges();

            return NoContent();

        }
        
    }
}
