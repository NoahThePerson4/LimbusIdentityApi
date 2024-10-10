using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class ExtraSinnerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OffenseLevel",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Support",
                table: "Passives",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fatal",
                table: "Identities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ineffective",
                table: "Identities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OffenseLevel",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Support",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "Fatal",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Ineffective",
                table: "Identities");
        }
    }
}
