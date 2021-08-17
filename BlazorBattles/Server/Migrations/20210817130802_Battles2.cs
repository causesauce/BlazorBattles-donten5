using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorBattles.Server.Migrations
{
    public partial class Battles2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpponrntId",
                table: "Battles");

            migrationBuilder.AlterColumn<int>(
                name: "OpponentId",
                table: "Battles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OpponentId",
                table: "Battles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OpponrntId",
                table: "Battles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
