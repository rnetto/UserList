using System.Collections.Generic;

namespace UserList.Models.DTOs
{
    public class UsuarioListaDTO
    {
        public List<Usuario> ListaUsuarios;
        public int? TotalPaginas { get; set; }
        public int? PagAtual { get; set; }
    }
}
