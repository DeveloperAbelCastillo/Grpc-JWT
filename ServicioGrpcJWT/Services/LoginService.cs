using Grpc.Core;
using LoginServicio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServicioGrpcJWT.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static LoginServicio.Login;


namespace ServicioGrpcJWT.Services
{
    public class LoginService : LoginBase
    {
        private readonly ILogger<LoginService> _logger;
        private readonly JWTContext _context;
        private readonly IConfiguration _configuration;
        public LoginService(ILogger<LoginService> logger, JWTContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public override async Task<Token> IniciarSesion(UsuarioLogin request, ServerCallContext context)
        {
            try
            {
                var userAgent = context.RequestHeaders.GetValue("user-agent");
                var httpContext = context.GetHttpContext();
                var clientCertificate = httpContext.Connection.ClientCertificate;
                var user = context.GetHttpContext().User;

                var _userInfo = await AutenticarUsuarioAsync(request.Usuario, request.Password);
                if (_userInfo != null)
                {
                    return new Token(){ Token_ = GenerarTokenJWT(_userInfo) };
                }
                else
                {
                    return new Token() { Token_ = "" };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // COMPROBAMOS SI EL USUARIO EXISTE EN LA BASE DE DATOS 
        private async Task<Entidades.UsuarioInfo> AutenticarUsuarioAsync(string usuario, string password)
        {
            // AQUÍ LA LÓGICA DE AUTENTICACIÓN //

            // Supondremos que el Usuario existe en la Base de Datos.
            // Retornamos un objeto del tipo UsuarioInfo, con toda
            // la información del usuario necesaria para el Token.
            return new Entidades.UsuarioInfo()
            {
                // Id del Usuario en el Sistema de Información (BD)
                Id = new Guid(),
                Nombre = "Abel",
                Apellidos = "Castillo Landeros",
                Email = "acastillo@gmail.com",
                Rol = "Administrador"
            };

            // Supondremos que el Usuario NO existe en la Base de Datos.
            // Retornamos NULL.
            //return null;
        }

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(Entidades.UsuarioInfo usuarioInfo)
        {
            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()),
                new Claim("nombre", usuarioInfo.Nombre),
                new Claim("apellidos", usuarioInfo.Apellidos),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email),
                new Claim(ClaimTypes.Role, usuarioInfo.Rol)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 24 horas.
                    expires: DateTime.UtcNow.AddHours(24)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}
