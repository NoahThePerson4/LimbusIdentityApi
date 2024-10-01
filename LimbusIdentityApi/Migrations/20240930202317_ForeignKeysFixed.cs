using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeysFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passives_Identities_IdentityId",
                table: "Passives");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Identities_IdentityId",
                table: "Skills");

            migrationBuilder.AlterColumn<int>(
                name: "IdentityId",
                table: "Skills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdentityId",
                table: "Passives",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PassiveId",
                table: "Identities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Identities",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Passives_Identities_IdentityId",
                table: "Passives",
                column: "IdentityId",
                principalTable: "Identities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Identities_IdentityId",
                table: "Skills",
                column: "IdentityId",
                principalTable: "Identities",
                principalColumn: "Id");
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

            migrationBuilder.DropColumn(
                name: "PassiveId",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Identities");

            migrationBuilder.AlterColumn<int>(
                name: "IdentityId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdentityId",
                table: "Passives",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
