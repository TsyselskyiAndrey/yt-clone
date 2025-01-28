using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeoServer.Migrations
{
    /// <inheritdoc />
    public partial class PlaylistLastUpdatedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb96e34f-dec8-4f53-ac1e-bdc6f6af85f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f36bdc93-5503-409b-bc9d-b60f007460a9");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Playlists",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38985dfe-f11b-4c21-8b78-b3e0a835b5bb", null, "admin", "admin" },
                    { "f38f67a0-0419-42c1-bea2-5f60a13168df", null, "channel_owner", "channel_owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38985dfe-f11b-4c21-8b78-b3e0a835b5bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f38f67a0-0419-42c1-bea2-5f60a13168df");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Playlists");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eb96e34f-dec8-4f53-ac1e-bdc6f6af85f3", null, "admin", "admin" },
                    { "f36bdc93-5503-409b-bc9d-b60f007460a9", null, "channel_owner", "channel_owner" }
                });
        }
    }
}
