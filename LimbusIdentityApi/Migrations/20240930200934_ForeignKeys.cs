using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int?>(
                name: "IdentityId",
                table: "Skills",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int?>(
                name: "IdentityId",
                table: "Passives",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_IdentityId",
                table: "Skills",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Passives_IdentityId",
                table: "Passives",
                column: "IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passives_Identities_IdentityId",
                table: "Passives",
                column: "IdentityId",
                principalTable: "Identities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Identities_IdentityId",
                table: "Skills",
                column: "IdentityId",
                principalTable: "Identities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passives_Identities_IdentityId",
                table: "Passives");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Identities_IdentityId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_IdentityId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Passives_IdentityId",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Passives");
        }
    }
}
