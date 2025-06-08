using System;
using System.Collections.Generic;
using Codigo_De_Barra.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Codigo_De_Barra.Database;

public partial class ProdutosDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }

    public ProdutosDbContext(DbContextOptions<ProdutosDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chave composta para PedidoProduto
        modelBuilder.Entity<PedidoProduto>()
            .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

        // Relacionamento: Pedido -> PedidoProduto
        modelBuilder.Entity<PedidoProduto>()
            .HasOne(pp => pp.Pedido)
            .WithMany(p => p.PedidoProdutos)
            .HasForeignKey(pp => pp.PedidoId);

        // Relacionamento: Produto -> PedidoProduto
        modelBuilder.Entity<PedidoProduto>()
            .HasOne(pp => pp.Produto)
            .WithMany() // ou .WithMany(p => p.PedidoProdutos) se existir a navegação no Produto
            .HasForeignKey(pp => pp.ProdutoId);

        base.OnModelCreating(modelBuilder);
    }
}
