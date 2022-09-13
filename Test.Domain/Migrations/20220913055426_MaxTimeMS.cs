using Microsoft.EntityFrameworkCore.Migrations;

namespace Test.Domain.Migrations
{
    public partial class MaxTimeMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percent",
                table: "Queries");

            migrationBuilder.AddColumn<long>(
                name: "MaxTimeMS",
                table: "Queries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTimeMS",
                table: "Queries");

            migrationBuilder.AddColumn<float>(
                name: "Percent",
                table: "Queries",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
