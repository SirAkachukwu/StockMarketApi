using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockMarketApi.Migrations
{
    /// <inheritdoc />
    public partial class CommentOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3a95968-31f5-4aa7-927b-4e1af294016a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2271e2e-8ea7-42b2-8e58-49666819d7b3");

            migrationBuilder.AddColumn<string>(
                name: "ApiUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "74b37059-537e-4ed0-90bb-9350e04dc603", null, "User", "USER" },
                    { "8765d589-99ae-4589-8895-f653efff7b4e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApiUserId",
                table: "Comments",
                column: "ApiUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApiUserId",
                table: "Comments",
                column: "ApiUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApiUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApiUserId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74b37059-537e-4ed0-90bb-9350e04dc603");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8765d589-99ae-4589-8895-f653efff7b4e");

            migrationBuilder.DropColumn(
                name: "ApiUserId",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3a95968-31f5-4aa7-927b-4e1af294016a", null, "User", "USER" },
                    { "d2271e2e-8ea7-42b2-8e58-49666819d7b3", null, "Admin", "ADMIN" }
                });
        }
    }
}
