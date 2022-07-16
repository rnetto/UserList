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
        internal async Task<UsuarioListaDTO> BuscarLista(UsuarioDTO user, int skip, int take)
        {
            var cont = await _context.Usuario.AsNoTracking().ToListAsync();
            var total = 0;
            var query = new List<Usuario>();

            if (user.DtInicioF.HasValue && !user.DtFimF.HasValue)
            {
                total = cont.Count(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco)
                        && us.DataCadastro >= user.DtInicioF);

                query = await _context.Usuario
                    .Where(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco)
                        && us.DataCadastro >= user.DtInicioF)
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            else if (!user.DtInicioF.HasValue && user.DtFimF.HasValue)
            {
                total = cont.Count(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco)
                        && us.DataCadastro <= user.DtFimF);

                query = await _context.Usuario
                    .Where(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco)
                        && us.DataCadastro <= user.DtFimF)
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            else if (user.DtInicioF.HasValue && user.DtFimF.HasValue)
            {
                total = cont.Count(us => us.Nome.Contains(user.Nome)
                                        && us.Apelido.Contains(user.Apelido)
                                        && us.Telefone.Contains(user.Telefone)
                                        && us.Endereco.Contains(user.Endereco)
                                        && us.DataCadastro >= user.DtInicioF
                                        && us.DataCadastro <= user.DtFimF);

                query = await _context.Usuario
                    .Where(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco)
                        && us.DataCadastro >= user.DtInicioF
                        && us.DataCadastro <= user.DtFimF)
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            else
            {
                total = cont.Count(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco));

                query = await _context.Usuario
                    .Where(us => us.Nome.Contains(user.Nome)
                        && us.Apelido.Contains(user.Apelido)
                        && us.Telefone.Contains(user.Telefone)
                        && us.Endereco.Contains(user.Endereco))
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }

            var retorno = new UsuarioListaDTO()
            {
                ListaUsuarios = query,
                TotalPaginas = total,
                PagAtual = skip
            };

            return retorno;
        }

        internal async Task<UsuarioDTO> BuscaUsuario(int? id)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);

            var user = new UsuarioDTO()
            {
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Endereco = usuario.Endereco,
                Telefone = usuario.Telefone,
                DataCadastro = (DateTime)usuario.DataCadastro
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

        internal async Task DeletarAsync(int id)
        {
            var user = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);

            _context.Usuario.Remove(user);
            _context.SaveChanges();
        }
    }
}
