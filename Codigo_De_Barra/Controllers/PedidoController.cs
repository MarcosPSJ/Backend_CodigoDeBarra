using Codigo_De_Barra.Database;
using Codigo_De_Barra.DTO;
using Codigo_De_Barra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(dbContext.Pedidos.Include(p => p.Cliente).Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto).OrderByDescending(p => p.DataPedido));
        }

        [HttpGet("{id}")]
        public ActionResult<Pedido> GetPedido(string id)
        {
            Pedido? pedido = dbContext.Pedidos
                .Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto)
                .Include(p => p.Cliente)
                .FirstOrDefault(p => p.Id == id);

            if (pedido is null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpGet("pedidosPorCliente/{idCliente}")]
        public ActionResult<IEnumerable<Pedido>> GetPedidosPorCliente(string idCliente)
        {
            Cliente? cliente = dbContext.Clientes.FirstOrDefault(c => c.Id == idCliente);

            if (cliente is null)
            {
                return BadRequest("Usuário não encontrado");
            }

            IEnumerable<Pedido> pedidos = dbContext.Pedidos
                .Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto)
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Id.Equals(idCliente))
                .OrderByDescending(p => p.DataPedido);

            if (!pedidos.Any())
            {
                return NoContent();
            }

            return Ok(pedidos);
        }

        [HttpPost]
        public ActionResult<Pedido> CreatePedido(PedidoDTO novoPedidoDTO)
        {
            if (novoPedidoDTO.produtos == null || !novoPedidoDTO.produtos.Any())
            {
                return BadRequest("É necessário enviar a lista de produtos");
            }

            Cliente? cliente = dbContext.Clientes.FirstOrDefault(c => c.Cpf == novoPedidoDTO.clientecpf);
            if (cliente == null)
            {
                return BadRequest("Cliente inválido");
            }

            var codigos = novoPedidoDTO.produtos.Select(p => p.CodigoDeBarra).ToList();
            var produtosEncontrados = dbContext.Produtos.Where(p => codigos.Contains(p.CodigoDeBarra)).ToList();

            if (produtosEncontrados.Count != novoPedidoDTO.produtos.Count)
            {
                return BadRequest("Um ou mais produtos não foram encontrados");
            }

            Pedido novoPedido = new Pedido(cliente, DateTime.Now);

            foreach (var produtoDTO in novoPedidoDTO.produtos)
            {
                var produto = produtosEncontrados.FirstOrDefault(p => p.CodigoDeBarra == produtoDTO.CodigoDeBarra);
                if (produto != null)
                {
                    novoPedido.PedidoProdutos.Add(new PedidoProduto
                    {
                        Produto = produto
                    });
                }
            }

            dbContext.Pedidos.Add(novoPedido);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(CreatePedido), novoPedido);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePedido(string id, PedidoDTO pedidoAtualizadoDTO)
        {
            var pedido = dbContext.Pedidos
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound("Pedido não encontrado");
            }

            if (pedidoAtualizadoDTO.produtos == null || !pedidoAtualizadoDTO.produtos.Any())
            {
                return BadRequest("É necessário enviar a lista de produtos");
            }

            // Remove produtos antigos
            pedido.PedidoProdutos.Clear();

            var codigos = pedidoAtualizadoDTO.produtos.Select(p => p.CodigoDeBarra).ToList();
            var produtosEncontrados = dbContext.Produtos.Where(p => codigos.Contains(p.CodigoDeBarra)).ToList();

            foreach (var produtoDTO in pedidoAtualizadoDTO.produtos)
            {
                var produto = produtosEncontrados.FirstOrDefault(p => p.CodigoDeBarra == produtoDTO.CodigoDeBarra);
                if (produto != null)
                {
                    pedido.PedidoProdutos.Add(new PedidoProduto
                    {
                        Produto = produto
                    });
                }
            }

            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePedido(string id)
        {
            var pedido = dbContext.Pedidos
                .Include(p => p.PedidoProdutos)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            pedido.PedidoProdutos.Clear();
            dbContext.Pedidos.Remove(pedido);
            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/deleteClientePedido/{idCliente}")]
        public IActionResult DeleteClientePedido(string idCliente)
        {
            var cliente = dbContext.Clientes.FirstOrDefault(c => c.Id == idCliente);
            if (cliente == null)
            {
                return BadRequest("Usuário não encontrado");
            }

            var pedido = dbContext.Pedidos
                .Include(p => p.PedidoProdutos)
                .FirstOrDefault(p => p.Cliente.Id.Equals(idCliente));

            if (pedido == null)
            {
                return NoContent();
            }

            pedido.PedidoProdutos.Clear();
            dbContext.Pedidos.Remove(pedido);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
