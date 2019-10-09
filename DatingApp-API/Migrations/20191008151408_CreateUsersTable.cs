using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class CreateUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", nullable: false),
                    ImagePath = table.Column<string>(type: "varchar(100)", nullable: false, defaultValue: "none")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_users");
        }
    }
}
