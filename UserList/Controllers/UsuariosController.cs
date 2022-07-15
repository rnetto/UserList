using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserList.Data;
using UserList.Models;
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
        //[Route("usuarios/Filtrar")]        
        //[Route("usuarios/{skip}/{take}")]        
        public async Task<IActionResult> Filtrar(UsuarioDTO usuarioDTO, int skip = 0, int take = 10)
        {
            try
            {
                take = take > 30 ? take = 30 : take;
                usuarioDTO.Nome = "";

                var listaUsuario = await _usuarioService.GetAsync(usuarioDTO, skip, take);

                return View("_ListUserPartial", listaUsuario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

        }



        public async Task<IActionResult> Detalhes(int? id)
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

        public IActionResult CriarNovoUsuario()
        {
            return View("_CriarNovoUsuarioPartial");
        }

        [HttpPost]
        public async Task<IActionResult> SalvarCadastro(UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
