﻿using System;

namespace UserList.Models.DTOs
{
    public class UsuarioDTO
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DtInicioF { get; set; }
        public DateTime? DtFimF { get; set; }
    }
}
