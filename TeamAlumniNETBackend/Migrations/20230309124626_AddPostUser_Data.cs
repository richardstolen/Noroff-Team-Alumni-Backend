using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    public partial class AddPostUser_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Body", "GroupId", "LastUpdate", "TargetEvent", "TargetGroup", "TargetPost", "TargetTopic", "TargetUser", "Title", "TopicId", "UserId" },
                values: new object[] { 1, "Invite to all who like football to watch the match", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, 1, 1, "Footbal Match", null, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { "Love Futsal and footbal", "never watched a whole footballmatch", "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg", "Attending Experis Academy courses at Noroff", "Richardinho" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { "Happy boy", null, null, null, "richardinho" });
        }
    }
}
