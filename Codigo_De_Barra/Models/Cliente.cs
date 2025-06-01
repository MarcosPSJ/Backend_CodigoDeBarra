﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codigo_De_Barra.Models
{
    public class Cliente
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Cliente(string nome, string cpf, string email, string senha)
        {
            this.Nome = nome;
            this.Cpf = cpf;
            this.Email = email;
            this.Senha = senha;
        }
        private Cliente() { }
    }
}
