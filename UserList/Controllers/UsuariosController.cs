using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

                var listaUsuario = await _usuarioService.BuscarLista(usuarioDTO, skip, take);

                return PartialView("_ListaUsuarioPartial", listaUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult CriarNovoUsuario()
        {
            var usuario = new UsuarioDTO();
            return View("_CadastroUsuarioPartial", usuario);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarCadastro(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO.Nome.Length < 3)
                    throw new Exception("Campo <strong>NOME</strong> não pode ser vazio.");
                if (usuarioDTO.Telefone.Length < 10)
                    throw new Exception("Campo <strong>TELEFONE</strong> não pode ser vazio.");

                if (usuarioDTO.Id.HasValue && _usuarioService.Existe((int)usuarioDTO.Id))
                {
                    await _usuarioService.Atualizar(usuarioDTO);
                    return Ok("Cadastro atualizado");
                }
                else
                {
                    await _usuarioService.Adicionar(usuarioDTO);
                    return Ok("Novo cadastro realizado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("<strong>Usuario-Id</strong> não encontrado");
                }

                var usuario = await _usuarioService.BuscaUsuario(id);

                if (usuario == null)
                {
                    throw new Exception("<strong>Usuario</strong> não encontrado");
                }

                return PartialView("_CadastroUsuarioPartial", usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound("<strong>Usuario-Id</strong> não encontrado");
                }

                var usuario = await _usuarioService.BuscaUsuario(id);

                if (usuario == null)
                {
                    return NotFound("<strong>Usuario</strong> não encontrado");
                }

                await _usuarioService.Deletar((int)id);

                return Ok("Cadastro cancelado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
