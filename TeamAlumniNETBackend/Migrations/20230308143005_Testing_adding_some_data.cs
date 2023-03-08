using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    public partial class Testing_adding_some_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { 1, "Happy boy", null, null, null, "richardinho" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
