using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeoServer.Migrations
{
    /// <inheritdoc />
    public partial class IsUploadedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b19bc27-4be8-47d3-ae68-c7fd5f3cdcb4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87c7b497-3332-4933-9779-7047e78c9157");

            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "65a7b577-d3cd-4624-a410-68f95078d031", null, "admin", "admin" },
                    { "673cc5ae-11e2-4682-a545-539703720a0e", null, "channel_owner", "channel_owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65a7b577-d3cd-4624-a410-68f95078d031");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673cc5ae-11e2-4682-a545-539703720a0e");

            migrationBuilder.DropColumn(
                name: "IsUploaded",
                table: "Videos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b19bc27-4be8-47d3-ae68-c7fd5f3cdcb4", null, "admin", "admin" },
                    { "87c7b497-3332-4933-9779-7047e78c9157", null, "channel_owner", "channel_owner" }
                });
        }
    }
}
