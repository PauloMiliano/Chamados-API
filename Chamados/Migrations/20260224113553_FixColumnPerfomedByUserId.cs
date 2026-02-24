using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamados.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnPerfomedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PerformedBy",
                table: "TicketHistories",
                newName: "PerformedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PerformedByUserId",
                table: "TicketHistories",
                newName: "PerformedBy");
        }
    }
}
