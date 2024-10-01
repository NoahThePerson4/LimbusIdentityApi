using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class NewForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassiveId",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Identities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassiveId",
                table: "Identities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillId",
                table: "Identities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
