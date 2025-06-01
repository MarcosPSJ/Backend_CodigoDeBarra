using Codigo_De_Barra.Database;
using Codigo_De_Barra.DTO;
using Codigo_De_Barra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codigo_De_Barra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ProdutosDbContext dbContext;
        public ClienteController(ProdutosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClienteDTOOutput>> GetCliente()
        {
            IEnumerable<ClienteDTOOutput> clientes = dbContext
                .Clientes
                .Select(
                    c => new ClienteDTOOutput(c.Id, c.Nome, c.Cpf, c.Email)
                );

            return Ok(clientes);
        }


        [HttpGet("{id}")]
        public ActionResult<Cliente> GetClienteId(string id)
        {
            Cliente? cliente = dbContext
                .Clientes
                .FirstOrDefault(c => c.Id == id);
            if (cliente is null)
            {
                return NotFound();
            }

            ClienteDTOOutput clienteDTO = new ClienteDTOOutput(cliente.Id, cliente.Nome, cliente.Cpf, cliente.Email);

            return Ok(cliente);
        }


        [HttpPost]
        public ActionResult<ClienteDTOInput> CreateCliente(ClienteDTOInput novoClienteDTO)
        {
            if (dbContext.Clientes.Any(cliente => cliente.Cpf.Equals(novoClienteDTO.cpf)))
            {
                return BadRequest("Já existe um cliente com este CPF");
            }

            Cliente novoCliente = new Cliente(novoClienteDTO.nome, novoClienteDTO.cpf, novoClienteDTO.email, novoClienteDTO.senha); 

            dbContext.Clientes.Add(novoCliente);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(CreateCliente), novoCliente);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateCliente(string id, ClienteDTOOutput clienteAtualizadoDTO)
        {
            Cliente? clienteEncontrado =
                dbContext
                .Clientes
                .FirstOrDefault(c => c.Id == id);

            if (clienteEncontrado is null)
            {
                return NotFound();
            }
            if (dbContext.Clientes.Any(cliente => cliente.Id != id && cliente.Cpf.Equals(clienteAtualizadoDTO.Cpf)))
            {
                return BadRequest("Já existe um cliente com esse CPF");
            }

            clienteEncontrado.Nome = clienteAtualizadoDTO.Nome;
            clienteEncontrado.Cpf = clienteAtualizadoDTO.Cpf;
            clienteEncontrado.Email = clienteAtualizadoDTO.Email;
            clienteEncontrado.Senha = clienteAtualizadoDTO.Senha;

            dbContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{cpf}")]
        public IActionResult DeleteProduto(string cpf)
        {
            Cliente? clienteEncontrado =
                dbContext
                .Clientes
                .FirstOrDefault(c => c.Cpf == cpf);

            if (clienteEncontrado == null)
            {
                return NotFound();
            }

            dbContext.Clientes.Remove(clienteEncontrado);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
