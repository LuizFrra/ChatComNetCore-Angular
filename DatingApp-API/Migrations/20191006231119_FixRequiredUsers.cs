using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.Migrations
{
    public partial class FixRequiredUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "tbl_users",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tbl_users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "tbl_users",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "tbl_users");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "tbl_users",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tbl_users",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
