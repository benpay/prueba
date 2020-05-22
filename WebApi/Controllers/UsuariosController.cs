using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public UsuariosController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios (recupera todos los usuarios)
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarioItems()
        //{
        //    return await _context.UsuarioItems.ToListAsync();
        //}

        // GET: api/Usuarios/5 (recupera el usuario por id)
        /** No está visible para que se realice la búsqueda por email, se puede volver a activar descomentando la etiqueta de HttpGet y haciendo cambiando la visibilidad a public  **/
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.UsuarioItems.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: api/Usuarios/correo@direcion.com (recupera por dirección de correo)
        [HttpGet("byEmail/{email}/{password}")]
        public async Task<ActionResult<Usuario>> GetUsuarioMail(string email, string password)
        {
            //var usuario = await _context.UsuarioItems.FirstOrDefaultAsync(x => x.Email.Contains(email));
            var usuario = await _context.UsuarioItems.FirstOrDefaultAsync(x => x.Email.Contains(email));

            if (usuario == null)
            {
                return NotFound();
            }
            else if (usuario.Password != password)
            {
                return Unauthorized();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5 (actualiza usuario por id)
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificacionUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios (añade un nuevo usuario)
        [HttpPost]
        public async Task<ActionResult<Usuario>> AltaUsuario(Usuario usuario)
        {
            //if (RegexUtilities.IsValidEmail(usuario.Email))
            //{
                _context.UsuarioItems.Add(usuario);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
            //}
            //return BadRequest();
        }

        // DELETE: api/Usuarios/5 (borra un usuario existente)
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Usuario>> BorrarUsuario(int id)
        //{
        //    var usuario = await _context.UsuarioItems.FindAsync(id);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UsuarioItems.Remove(usuario);
        //    await _context.SaveChangesAsync();

        //    return usuario;
        //}

        // Método privado para saber si un usuario existe o no por id
        private bool UsuarioExists(int id)
        {
            return _context.UsuarioItems.Any(e => e.Id == id);
        }
    }
}
