using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserList.Data;
using UserList.Models;
using UserList.Models.DTOs;

namespace UserList.Service
{
    public class UsuariosService
    {
        private readonly UserListContext _context;
        public UsuariosService(UserListContext context)
        {
            _context = context;
        }
        public async Task<UsuarioListaDTO> GetAsync(UsuarioDTO user, int skip, int take) 
        {

            var total = _context.Usuario.ToList();
                
            var totalPag = total.Count(us => user.Nome.Any() || us.Nome.Contains(user.Nome) 
                                    //&& us.Apelido.Contains(user.Apelido)
                                    //&& us.Telefone.Contains(user.Telefone)
                                    //&& us.Endereco.Contains(user.Endereco)
                                    //&& us.DataCadastro >= user.DtInicioF
                                    //&& us.DataCadastro <= user.DtFimF
                                    );
            
            var query = await _context.Usuario.ToListAsync();
            //                    .Where(us => us.Nome.Contains(user.Nome)
            //                        && us.Apelido.Contains(user.Apelido)
            //                        && us.Telefone.Contains(user.Telefone)
            //                        && us.Endereco.Contains(user.Endereco)
            //                        && us.DataCadastro >= user.DtInicioF
            //                        && us.DataCadastro <= user.DtFimF)
            //                    .AsNoTracking()
            //                    .Skip(skip)
            //                    .Take(take)
            //                    .ToListAsync();

            var retorno = new UsuarioListaDTO()
            {
                ListaUsuarios = query,
                TotalPaginas = totalPag,
                PagAtual = skip
            };

            return retorno;
        }
    }
}
