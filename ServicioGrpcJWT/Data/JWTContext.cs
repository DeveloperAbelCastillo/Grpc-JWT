using Microsoft.EntityFrameworkCore;
using ServicioGrpcJWT.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioGrpcJWT.Data
{
    public class JWTContext : DbContext
    {
        public JWTContext(DbContextOptions<JWTContext> options)
            : base(options)
        {
        }

        public DbSet<Pais> Pais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().HasData(
                new Pais { Nombre = "España", Habitantes = 46000000 },
                new Pais { Nombre = "Alemania", Habitantes = 83000000 },
                new Pais { Nombre = "Francia", Habitantes = 65000000 },
                new Pais { Nombre = "Italia", Habitantes = 61000000 },
                new Pais { Nombre = "Mexico", Habitantes = 96000000 },
                new Pais { Nombre = "EUA", Habitantes = 63000000 }
                );
        }
    }
}
