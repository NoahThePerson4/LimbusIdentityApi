using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class FactionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Faction",
                table: "Identities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faction",
                table: "Identities");
        }
    }
}
