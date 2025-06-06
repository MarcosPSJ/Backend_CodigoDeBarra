﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codigo_De_Barra.Models
{
    public class Pedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public Cliente Cliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get {
                return this.Produtos.Sum(p => p.Preco);
            }
        }

        public Pedido(List<Produto> produtos, Cliente nomeCliente, DateTime data)
        {
            this.Produtos = produtos;
            this.Cliente = nomeCliente;
            this.DataPedido = data;
        }

        private Pedido() { }
    }
}
