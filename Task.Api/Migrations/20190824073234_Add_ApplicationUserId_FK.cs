using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Api.Migrations
{
    public partial class Add_ApplicationUserId_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_AspNetUsers_AssignedToId",
                table: "WorkItems");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1682b138-e299-4766-a916-5405483e4286");

            migrationBuilder.RenameColumn(
                name: "AssignedToId",
                table: "WorkItems",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_AssignedToId",
                table: "WorkItems",
                newName: "IX_WorkItems_ApplicationUserId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0ab1b105-9587-4c64-9c3c-59b089ae1d69", 0, "Mohammadpur", "e2872b2c-c72f-45ee-88b3-13336a5bc135", "trahman@ael-bd.com", false, "Tanvir", "Rahman", false, null, null, null, null, null, false, null, false, "trahman@ael-bd.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_AspNetUsers_ApplicationUserId",
                table: "WorkItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_AspNetUsers_ApplicationUserId",
                table: "WorkItems");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ab1b105-9587-4c64-9c3c-59b089ae1d69");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "WorkItems",
                newName: "AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ApplicationUserId",
                table: "WorkItems",
                newName: "IX_WorkItems_AssignedToId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1682b138-e299-4766-a916-5405483e4286", 0, "Mohammadpur", "3b28967f-6a5c-42ee-a356-d7a1e5d6bf02", "trahman@ael-bd.com", false, "Tanvir", "Rahman", false, null, null, null, null, null, false, null, false, "trahman@ael-bd.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_AspNetUsers_AssignedToId",
                table: "WorkItems",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
