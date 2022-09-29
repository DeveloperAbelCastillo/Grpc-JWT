using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioGrpcJWT.Entidades
{
    public class UsuarioInfo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class UsuarioLogin
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
