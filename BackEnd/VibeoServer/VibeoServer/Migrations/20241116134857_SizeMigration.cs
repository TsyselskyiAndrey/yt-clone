using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeoServer.Migrations
{
    /// <inheritdoc />
    public partial class SizeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65a7b577-d3cd-4624-a410-68f95078d031");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673cc5ae-11e2-4682-a545-539703720a0e");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Videos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eb96e34f-dec8-4f53-ac1e-bdc6f6af85f3", null, "admin", "admin" },
                    { "f36bdc93-5503-409b-bc9d-b60f007460a9", null, "channel_owner", "channel_owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb96e34f-dec8-4f53-ac1e-bdc6f6af85f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f36bdc93-5503-409b-bc9d-b60f007460a9");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Videos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "65a7b577-d3cd-4624-a410-68f95078d031", null, "admin", "admin" },
                    { "673cc5ae-11e2-4682-a545-539703720a0e", null, "channel_owner", "channel_owner" }
                });
        }
    }
}
