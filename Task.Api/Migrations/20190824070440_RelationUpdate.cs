using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Api.Migrations
{
    public partial class RelationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89d91512-9fde-4871-a5e2-d23309e9d802");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1682b138-e299-4766-a916-5405483e4286", 0, "Mohammadpur", "3b28967f-6a5c-42ee-a356-d7a1e5d6bf02", "trahman@ael-bd.com", false, "Tanvir", "Rahman", false, null, null, null, null, null, false, null, false, "trahman@ael-bd.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1682b138-e299-4766-a916-5405483e4286");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "89d91512-9fde-4871-a5e2-d23309e9d802", 0, "Mohammadpur", "b1c9c4a0-769a-44bb-b47d-725ef2230d3b", "trahman@ael-bd.com", false, "Tanvir", "Rahman", false, null, null, null, null, null, false, null, false, "trahman@ael-bd.com" });
        }
    }
}
