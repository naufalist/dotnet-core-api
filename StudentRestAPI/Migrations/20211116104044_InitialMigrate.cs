using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StudentRestAPI.Migrations
{
  public partial class InitialPostgres : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Student",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            LastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            IPK = table.Column<decimal>(type: "numeric(4,2)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Student", x => x.Id);
          });

      migrationBuilder.InsertData(
          table: "Student",
          columns: new[] { "Id", "FirstName", "IPK", "LastName" },
          values: new object[,]
          {
                    { 1, "Zaky", 3.50m, "Ramadhan" },
                    { 2, "Devina", 4.00m, "Ramadhani" },
                    { 3, "Putri", 3.35m, "Larasati" }
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Student");
    }
  }
}
