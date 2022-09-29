using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaisServicio;
using ServicioGrpcJWT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PaisServicio.Paises;

namespace ServicioGrpcJWT.Services
{
    public class PaisService : PaisesBase
    {
        private readonly ILogger<PaisService> _logger;
        private readonly JWTContext _context;
        public PaisService(ILogger<PaisService> logger, JWTContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize] // SOLO USUARIOS AUTENTICADOS
        public override async Task ObtenerPaises(Empty request, IServerStreamWriter<Pais> responseStream, ServerCallContext context)
        {
            try
            {
                var userAgent = context.RequestHeaders.GetValue("user-agent");
                var httpContext = context.GetHttpContext();
                var clientCertificate = httpContext.Connection.ClientCertificate;
                var user = context.GetHttpContext().User;

                List<Entidades.Pais> paises = await _context.Pais.ToListAsync();

                if (paises.Count > 0)
                {
                    //recorrer result yh devolver por strem
                    foreach (Entidades.Pais pais in paises)
                    {
                        await responseStream.WriteAsync(new()
                        {
                            Id = pais.Id.ToString(),
                            Nombre = pais.Nombre,
                            Habitantes = pais.Habitantes
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
