using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamAlumniNETBackend.Migrations
{
    public partial class Changesinrsvp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventUser");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("14d229c9-5428-4456-8b54-8e9646103dfc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("ee1b03f4-ac5f-42d8-9562-4fc82e55197b"));

            migrationBuilder.DropColumn(
                name: "GuestCount",
                table: "Rsvps");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Rsvps");

            migrationBuilder.RenameColumn(
                name: "RsvpId",
                table: "Rsvps",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("06eaa822-17f3-4d74-8fb8-de1d03beb858"), "Striker at Manchester United", "Almost never score", "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620", "Attending Experis Academy courses at Noroff", "Kjetilinho" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("579da4b4-6443-4ccc-9391-4fd0ad6c4820"), "Love Futsal and footbal", "never watched a whole footballmatch", "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg", "Attending Experis Academy courses at Noroff", "Richardinho" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("06eaa822-17f3-4d74-8fb8-de1d03beb858"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("579da4b4-6443-4ccc-9391-4fd0ad6c4820"));

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rsvps",
                newName: "RsvpId");

            migrationBuilder.AddColumn<int>(
                name: "GuestCount",
                table: "Rsvps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Rsvps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EventUser",
                columns: table => new
                {
                    EventsEventId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUser", x => new { x.EventsEventId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_EventUser_Events_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("14d229c9-5428-4456-8b54-8e9646103dfc"), "Striker at Manchester United", "Almost never score", "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620", "Attending Experis Academy courses at Noroff", "Kjetilinho" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "FunFact", "Image", "Status", "UserName" },
                values: new object[] { new Guid("ee1b03f4-ac5f-42d8-9562-4fc82e55197b"), "Love Futsal and footbal", "never watched a whole footballmatch", "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg", "Attending Experis Academy courses at Noroff", "Richardinho" });

            migrationBuilder.CreateIndex(
                name: "IX_EventUser_UsersUserId",
                table: "EventUser",
                column: "UsersUserId");
        }
    }
}
