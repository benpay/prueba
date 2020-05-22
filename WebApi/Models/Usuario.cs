using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pais { get; set; }
        public int Telefono { get; set; }
        public int CodigoPostal { get; set; }
    }
}
