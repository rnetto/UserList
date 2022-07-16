using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserList.Data;
using UserList.Models.DTOs;
using UserList.Service;

namespace UserList.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserListContext _context;
        private readonly UsuariosService _usuarioService;


        public UsuariosController(UserListContext context)
        {
            _context = context;
            _usuarioService = new UsuariosService(_context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Filtrar(UsuarioDTO usuarioDTO, int skip = 0, int take = 10)
        {
            try
            {
                take = take > 30 ? take = 30 : take;

                usuarioDTO.Nome = usuarioDTO.Nome == null ? usuarioDTO.Nome = "" : usuarioDTO.Nome;
                usuarioDTO.Apelido = usuarioDTO.Apelido == null ? usuarioDTO.Apelido = "" : usuarioDTO.Apelido;
                usuarioDTO.Telefone = usuarioDTO.Telefone == null ? usuarioDTO.Telefone = "" : usuarioDTO.Telefone;
                usuarioDTO.Endereco = usuarioDTO.Endereco == null ? usuarioDTO.Endereco = "" : usuarioDTO.Endereco;

                var listaUsuario = await _usuarioService.BuscarLista(usuarioDTO, skip, take);

                return View("_ListaUsuarioPartial", listaUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        public IActionResult CriarNovoUsuario()
        {
            return View("_CriarNovoUsuarioPartial");
        }

        [HttpPost]
        public async Task<IActionResult> SalvarCadastro(UsuarioDTO usuario)
        {
            try
            {
                if (usuario.Nome.Any())
                    throw new Exception("Campo <strong>NOME</strong> não pode ser vazio.");
                if (usuario.Telefone.Any())
                    throw new Exception("Campo <strong>TELEFONE</strong> não pode ser vazio.");

                if(usuario.Id.HasValue)
                    await _usuarioService.Atualizar(usuario);
                else
                    await _usuarioService.Adicionar(usuario);
                 
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Informe um <strong>ID</strong> válido!");
                }

                var usuario = await _usuarioService.BuscaUsuario(id);

                if (usuario == null)
                {
                    throw new Exception("Usuário não existe.");
                }

                return View("_CriarNovoUsuarioPartial", usuario);
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

    }
}
