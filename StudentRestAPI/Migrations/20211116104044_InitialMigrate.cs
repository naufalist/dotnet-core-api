using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace StudentRestAPI.Migrations
{
    public partial class InitialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    IPK = table.Column<decimal>(type: "decimal(4,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "FirstName", "IPK", "LastName" },
                values: new object[] { 1, "Zaky", 3.50m, "Ramadhan" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "FirstName", "IPK", "LastName" },
                values: new object[] { 2, "Devina", 4.00m, "Ramadhani" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "FirstName", "IPK", "LastName" },
                values: new object[] { 3, "Putri", 3.35m, "Larasati" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
