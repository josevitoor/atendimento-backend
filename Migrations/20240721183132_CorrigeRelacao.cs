using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtendimentoBackend.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeRelacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_DatasSemana_DataSemanaId",
                table: "Agendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Servicos_ServicoId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_DataSemanaId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "DataSemanaId",
                table: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "ServicoId",
                table: "Agendamentos",
                newName: "DataServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Agendamentos_ServicoId",
                table: "Agendamentos",
                newName: "IX_Agendamentos_DataServicoId");

            migrationBuilder.AddColumn<string>(
                name: "Horario",
                table: "DatasSemana",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_DatasServicos_DataServicoId",
                table: "Agendamentos",
                column: "DataServicoId",
                principalTable: "DatasServicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_DatasServicos_DataServicoId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "Horario",
                table: "DatasSemana");

            migrationBuilder.RenameColumn(
                name: "DataServicoId",
                table: "Agendamentos",
                newName: "ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Agendamentos_DataServicoId",
                table: "Agendamentos",
                newName: "IX_Agendamentos_ServicoId");

            migrationBuilder.AddColumn<int>(
                name: "DataSemanaId",
                table: "Agendamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_DataSemanaId",
                table: "Agendamentos",
                column: "DataSemanaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_DatasSemana_DataSemanaId",
                table: "Agendamentos",
                column: "DataSemanaId",
                principalTable: "DatasSemana",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Servicos_ServicoId",
                table: "Agendamentos",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
