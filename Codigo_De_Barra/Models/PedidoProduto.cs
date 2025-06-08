using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codigo_De_Barra.Models
{
    public class PedidoProduto
    {
        [Key, Column(Order = 0)]
        public string PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        [Key, Column(Order = 1)]
        public string ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }

}
