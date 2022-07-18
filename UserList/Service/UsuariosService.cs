using Microsoft.EntityFrameworkCore;
using System;
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
        internal async Task<UsuarioListaDTO> BuscarLista(UsuarioDTO user, int skip, int take)
        {          
            var totalPag = await (from us in _context.Usuario
                            where (user.Nome == null || us.Nome.Contains(user.Nome))
                            && (user.Apelido == null || us.Apelido.Contains(user.Apelido))
                            && (user.Telefone == null || us.Telefone.Contains(user.Telefone))
                            && (user.Endereco == null || us.Endereco.Contains(user.Endereco))
                            && (user.DtInicioF == null || us.DataCadastro >= user.DtInicioF)
                            && (user.DtFimF == null || us.DataCadastro <= user.DtFimF)
                            select 1 ).CountAsync();

            var listaUsuario = await (from us in _context.Usuario
                                where (user.Nome == null || us.Nome.Contains(user.Nome))
                                && (user.Apelido == null || us.Apelido.Contains(user.Apelido))
                                && (user.Telefone == null || us.Telefone.Contains(user.Telefone))
                                && (user.Endereco == null || us.Endereco.Contains(user.Endereco))
                                && (user.DtInicioF == null || us.DataCadastro >= user.DtInicioF)
                                && (user.DtFimF == null || us.DataCadastro <= user.DtFimF)
                                orderby us.Nome, us.DataCadastro
                                select new UsuarioDTO
                                {
                                    Id = us.Id,
                                    Nome = us.Nome,
                                    Apelido = us.Apelido,
                                    Telefone = us.Telefone,
                                    Endereco = us.Endereco,
                                    DataCadastro = us.DataCadastro
                                })
                                .AsNoTracking()
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync();

            var retorno = new UsuarioListaDTO()
            {
                ListaUsuarios = listaUsuario,
                TotalPaginas = totalPag,
                PagAtual = skip
            };

            return retorno;
        }

        internal async Task<UsuarioDTO> BuscaUsuario(int? id)
        {
            var usuario = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var user = new UsuarioDTO()
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Endereco = usuario.Endereco,
                Telefone = usuario.Telefone,
                DataCadastro = usuario.DataCadastro
            };

            return user;
        }

        internal async Task Adicionar(UsuarioDTO usuario)
        {
            var user = new Usuario
            {
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Endereco = usuario.Endereco,
                Telefone = usuario.Telefone,
                DataCadastro = DateTime.Now.Date
            };

            await _context.Usuario.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        internal async Task Atualizar(UsuarioDTO usuario)
        {
            var user = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == usuario.Id);

            user.Nome = usuario.Nome;
            user.Apelido = usuario.Apelido;
            user.Endereco = usuario.Endereco;
            user.Telefone = usuario.Telefone;
            user.DataCadastro = (DateTime)usuario.DataCadastro;

            await _context.SaveChangesAsync();
        }

        internal bool Existe(int id)
        {
            var usuario = _context.Usuario.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (usuario == null)
                return false;
            else
                return true;
        }
        
        internal async Task Deletar(int id)
        {
            var user = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);

            _context.Usuario.Remove(user);
            _context.SaveChanges();
        }
    }
}
