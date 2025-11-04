using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelasDeSimulacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisciplinaAluno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscenteId = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplinaAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisciplinaAluno_Discentes_DiscenteId",
                        column: x => x.DiscenteId,
                        principalTable: "Discentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplinaAluno_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivroReserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscenteId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroReserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivroReserva_Discentes_DiscenteId",
                        column: x => x.DiscenteId,
                        principalTable: "Discentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroReserva_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaAluno_DiscenteId",
                table: "DisciplinaAluno",
                column: "DiscenteId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaAluno_DisciplinaId",
                table: "DisciplinaAluno",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_LivroReserva_DiscenteId",
                table: "LivroReserva",
                column: "DiscenteId");

            migrationBuilder.CreateIndex(
                name: "IX_LivroReserva_LivroId",
                table: "LivroReserva",
                column: "LivroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplinaAluno");

            migrationBuilder.DropTable(
                name: "LivroReserva");
        }
    }
}
