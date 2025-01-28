using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeoServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFormatMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308f278e-7284-458e-810e-9abe631df69a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5256dbdb-7f9c-4b79-b235-06e81a2e3fae");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Videos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b19bc27-4be8-47d3-ae68-c7fd5f3cdcb4", null, "admin", "admin" },
                    { "87c7b497-3332-4933-9779-7047e78c9157", null, "channel_owner", "channel_owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b19bc27-4be8-47d3-ae68-c7fd5f3cdcb4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87c7b497-3332-4933-9779-7047e78c9157");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "Videos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "308f278e-7284-458e-810e-9abe631df69a", null, "channel_owner", "channel_owner" },
                    { "5256dbdb-7f9c-4b79-b235-06e81a2e3fae", null, "admin", "admin" }
                });
        }
    }
}
