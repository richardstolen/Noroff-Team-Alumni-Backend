using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    public partial class ChangedEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("06eaa822-17f3-4d74-8fb8-de1d03beb858"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("579da4b4-6443-4ccc-9391-4fd0ad6c4820"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("1269cbc4-ff0f-4691-975d-814e1089df34"), "Striker at Manchester United", "Almost never score", "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620", "Attending Experis Academy courses at Noroff", "Kjetilinho" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("6e26163a-3337-4829-a225-c4d6cfc98d81"), "Love Futsal and footbal", "never watched a whole footballmatch", "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg", "Attending Experis Academy courses at Noroff", "Richardinho" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("1269cbc4-ff0f-4691-975d-814e1089df34"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("6e26163a-3337-4829-a225-c4d6cfc98d81"));

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Events");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("06eaa822-17f3-4d74-8fb8-de1d03beb858"), "Striker at Manchester United", "Almost never score", "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620", "Attending Experis Academy courses at Noroff", "Kjetilinho" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("579da4b4-6443-4ccc-9391-4fd0ad6c4820"), "Love Futsal and footbal", "never watched a whole footballmatch", "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg", "Attending Experis Academy courses at Noroff", "Richardinho" });
        }
    }
}
