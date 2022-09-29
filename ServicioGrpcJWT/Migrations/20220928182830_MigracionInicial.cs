using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioGrpcJWT.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Habitantes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pais",
                columns: new[] { "Id", "Habitantes", "Nombre" },
                values: new object[,]
                {
                    { new Guid("f240a38f-cf29-40a8-b94a-c5ee4863dbd0"), 46000000, "España" },
                    { new Guid("a551dc1e-8edb-454d-baed-45f710eb2033"), 83000000, "Alemania" },
                    { new Guid("6c26bb60-b483-41cb-af1d-fc6cb5bc040d"), 65000000, "Francia" },
                    { new Guid("90e98eb4-739b-44cc-a5be-c30d559950b2"), 61000000, "Italia" },
                    { new Guid("e8d28795-04c1-461c-bdb8-e2d56652e6d6"), 96000000, "Mexico" },
                    { new Guid("b086db89-a75b-40d2-b6ce-a9b7bff26d4f"), 63000000, "EUA" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pais");
        }
    }
}
