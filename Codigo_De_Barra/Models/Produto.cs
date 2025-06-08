﻿using Codigo_De_Barra.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemURL { get; set; }
    public string CodigoDeBarra { get; set; }

    public List<PedidoProduto> PedidoProdutos { get; set; } = new();

    public Produto(string nome, string descricao, decimal preco, string codigoDeBarra, string imagemURL)
    {
        this.Nome = nome;
        this.Descricao = descricao;
        this.Preco = preco;
        this.CodigoDeBarra = codigoDeBarra;
        this.ImagemURL = imagemURL;
    }

    private Produto() { }
}
