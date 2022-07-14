using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserList.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
