using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class SinAndSkillType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sin",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sin",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Skills");
        }
    }
}
