﻿using Codigo_De_Barra.Models;

namespace Codigo_De_Barra.DTO
{
    public class PedidoDTO
    {
        public string clientecpf { get; set; }
        public List<PedidoProdutoDTO> produtos { get; set; }
    }

}
