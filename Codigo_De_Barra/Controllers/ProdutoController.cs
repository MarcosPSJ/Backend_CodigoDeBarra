using Codigo_De_Barra.Database;
using Codigo_De_Barra.DTO;
using Codigo_De_Barra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Codigo_De_Barra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutosDbContext dbContext;
        public ProdutoController(ProdutosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            return Ok(dbContext.Produtos);
        }


        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutoId(string id)
        {
            Produto? produto = dbContext
                .Produtos
                .FirstOrDefault(p => p.CodigoDeBarra == id);

            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }


        [HttpGet("/codigoDeBarras/{codigoDeBarra}")]
        public ActionResult<IEnumerable<Produto>> GetProduto(string codigoDeBarra)
        {
            Produto? produto = dbContext
                .Produtos
                .FirstOrDefault(p => p.CodigoDeBarra == codigoDeBarra);
            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }


        [HttpPost]
        public ActionResult<Produto> CreateProduto(ProdutoDTO novoProdutoDTO)
        {
            Produto novoProduto = new Produto(novoProdutoDTO.nome, novoProdutoDTO.descricao, novoProdutoDTO.preco, novoProdutoDTO.codigoDeBarra, imagemURL: novoProdutoDTO.imagemURL);

            try
            {
                dbContext.Produtos.Add(novoProduto);
                dbContext.SaveChanges();
            }catch (Exception ex)
            {
                return BadRequest($"Erro ao criar produto: {ex.Message}");
            }



            return CreatedAtAction(nameof(CreateProduto), novoProduto);
        }

        [HttpPost("{id}/Upload")]
        public async Task<ActionResult<Produto>> UploadImage(string id, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("No image found");
            }

            Produto? produto = dbContext
                .Produtos
                .FirstOrDefault(p => p.Id == id);

            if (produto is null)
            {
                return NotFound();
            }

            string extensaoArquivo = arquivo.FileName.Substring(arquivo.FileName.LastIndexOf(".") + 1);

            string nomePasta = "produtos";
            string caminhoDaPastaDeUploads = Path.Combine("wwwroot", nomePasta);
            Directory.CreateDirectory(caminhoDaPastaDeUploads);

            string nomeDoArquivo = $"{id}.{extensaoArquivo}";
            string caminhoDoArquivo = Path.Combine(caminhoDaPastaDeUploads, nomeDoArquivo);

            using (var stream = new FileStream(caminhoDoArquivo, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            string urlServidor = $"{Request.Scheme}:{Request.Host}";

            string imagemUrl = $"{urlServidor}/{nomePasta}/{nomeDoArquivo}";

            produto.ImagemURL = imagemUrl;
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(UploadImage), produto);
        }



        [HttpPut("{id}")]
        public IActionResult UpdateProduto(string id, ProdutoDTO produtoAAtualizarDTO)
        {
            Produto? produtoEncontrado = 
                dbContext
                .Produtos
                .FirstOrDefault(p => p.Id == id);

            if (produtoEncontrado is null) 
            { 
                return NotFound(); 
            }
            if (dbContext.Produtos.Any(produto => produto.CodigoDeBarra == produtoAAtualizarDTO.codigoDeBarra && produto.Id != id )) // dbContext.Produtos.Any(produto => produto.Id != id)//)
            {
                return BadRequest("Codigo de barra já existente!");
            }

            produtoEncontrado.Nome = produtoAAtualizarDTO.nome;
            produtoEncontrado.Descricao = produtoAAtualizarDTO.descricao;
            produtoEncontrado.Preco = produtoAAtualizarDTO.preco;
            produtoEncontrado.CodigoDeBarra = produtoAAtualizarDTO.codigoDeBarra;
            dbContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduto(string id)
        {
            Produto? produtoEncontrado =
                dbContext
                .Produtos
                .FirstOrDefault(p => p.Id == id);
            
            if (produtoEncontrado == null)
            {
                return NotFound();
            }

            dbContext.Produtos.Remove(produtoEncontrado);
            dbContext.SaveChanges();

            return NoContent();
        }

    }
}
