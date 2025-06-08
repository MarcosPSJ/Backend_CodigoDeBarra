﻿using Codigo_De_Barra.Database;
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
            return Ok(dbContext.Produtos.OrderBy(p => p.Nome));
        }


        [HttpGet("idProduto/{id}")]
        public ActionResult<Produto> GetProdutoId(string id)
        {
            Produto? produto = dbContext
                .Produtos
                .FirstOrDefault(p => p.Id == id);

            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }


        [HttpGet("codigoDeBarras/{codigoDeBarra}")]
        public ActionResult<Produto> GetProdutoCodigoDeBarra(string codigoDeBarra)
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

        [HttpGet("nameProduct/{nomeProduto}")]
        public ActionResult<Produto> GetProdutoNome(string nomeProduto)
        {
            Produto? produto = dbContext
                .Produtos
                .FirstOrDefault(p => p.Nome == nomeProduto);
            if (produto is null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> CreateProduto([FromForm] ProdutoDTO novoProdutoDTO, IFormFile imagem)
        {

            if (dbContext.Produtos.Any(produto => produto.CodigoDeBarra == novoProdutoDTO.codigoDeBarra))
            {
                return BadRequest("Codigo de barra já existente!");
            }

            if (imagem == null || imagem.Length == 0)
            {
                return BadRequest("Imagem obrigatória");
            }

            string extensaoArquivo = Path.GetExtension(imagem.FileName);
            string nomePasta = "produtos";
            string caminhoDaPastaDeUploads = Path.Combine("wwwroot", nomePasta);
            Directory.CreateDirectory(caminhoDaPastaDeUploads);

            string nomeDoArquivo = $"{Guid.NewGuid()}{extensaoArquivo}";
            string caminhoDoArquivo = Path.Combine(caminhoDaPastaDeUploads, nomeDoArquivo);

            using (var stream = new FileStream(caminhoDoArquivo, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            string urlServidor = $"{Request.Scheme}:{Request.Host}";
            string imagemUrl = $"{urlServidor}/{nomePasta}/{nomeDoArquivo}";


            Produto novoProduto = new Produto(
                novoProdutoDTO.nome,
                novoProdutoDTO.descricao,
                novoProdutoDTO.preco,
                novoProdutoDTO.codigoDeBarra,
                imagemURL: imagemUrl
            );

            try
            {
                dbContext.Produtos.Add(novoProduto);
                dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar produto: {ex.Message}");
            }
            

            return CreatedAtAction(nameof(CreateProduto), novoProduto);
        }



        [HttpPatch("{id}")]
        public IActionResult UpdateProduto(string id, ProdutoDTO produtoAtualizadoDTO)
        {
            Produto? produtoEncontrado = 
                dbContext
                .Produtos
                .FirstOrDefault(p => p.Id == id);

            if (produtoEncontrado is null) 
            { 
                return NotFound(); 
            }
            if (dbContext.Produtos.Any(produto => produto.CodigoDeBarra == produtoAtualizadoDTO.codigoDeBarra && produto.Id != id ))
            {
                return BadRequest("Codigo de barra já existente!");
            }
            if (produtoAtualizadoDTO.nome != "string") { produtoEncontrado.Nome = produtoAtualizadoDTO.nome; }
            if (produtoAtualizadoDTO.descricao != "string") { produtoEncontrado.Descricao = produtoAtualizadoDTO.descricao; }
            if (produtoAtualizadoDTO.preco != 0) { produtoEncontrado.Preco = produtoAtualizadoDTO.preco; } // levando em consideração que o não se vendo prutos com preço 0
            if (produtoAtualizadoDTO.codigoDeBarra != "string") { produtoEncontrado.CodigoDeBarra = produtoAtualizadoDTO.codigoDeBarra; }

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
