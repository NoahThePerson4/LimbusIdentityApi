using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class SkillImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Skills");
        }
    }
}
