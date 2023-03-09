using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    public partial class AddGroupTopicEvent_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Description", "UserId" },
                values: new object[] { 1, "Football game", 2 });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupId", "Description", "Name" },
                values: new object[] { 1, "Group for members of Experis", "Experis" });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Description", "Name" },
                values: new object[] { 1, "Topic for people who love football", "Football" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { 2, "Striker at Manchester United", "Almost never score", "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620", "Attending Experis Academy courses at Noroff", "Kjetilinho" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "TopicId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Events");
        }
    }
}
