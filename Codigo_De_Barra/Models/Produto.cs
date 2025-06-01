using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codigo_De_Barra
{
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

        
        public Produto(string nome, string descricao, decimal preco, string codigoDeBarra)
        {
            this.Nome = nome;
            this.Descricao = descricao;
            this.Preco = preco;
            this.CodigoDeBarra = codigoDeBarra;
        }
        
        private Produto() { }
            
    }
}
