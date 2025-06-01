using Codigo_De_Barra.Database;
using Codigo_De_Barra.DTO;
using Codigo_De_Barra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            return Ok(dbContext.Pedidos);
        }

        [HttpGet("{id}")]
         public ActionResult<IEnumerable<Pedido>> GetPedido(string id) // inclued slide 
        {
            Pedido? pedido = dbContext
                .Pedidos
                .FirstOrDefault(p => p.Id == id);
                
            if (pedido is null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        public ActionResult<Pedido> CreatePedido(PedidoDTO novoPedidoDTO, string clienteIdPedidoDTO)
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
                .FirstOrDefault(cliente => cliente.Id == clienteIdPedidoDTO);

            if (cliente == null)
            {
                return BadRequest("Usuario inválidado");
            }

            Pedido novoPedido = new Pedido(produtos, cliente);

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
            
            
            //pedidoEncontrado.NomeCliente = pedidoAtualizadoDTO.cliente;
            //pedidoEncontrado.Produtos = pedidoAtualizadoDTO.produtosIds;
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
        // criar Delete de pedido por Cliente

    }
}
