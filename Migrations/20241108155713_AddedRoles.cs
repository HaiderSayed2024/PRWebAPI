using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3dd0ba8b-80c1-45f5-b3eb-6fe8ded39ff7", "1", "Admin", "HR" },
                    { "4e1f9d44-c2d5-4670-9d38-0309449061df", "1", "Admin", "User" },
                    { "a1bba028-b4e3-4312-95be-c50c09c2f795", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3dd0ba8b-80c1-45f5-b3eb-6fe8ded39ff7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e1f9d44-c2d5-4670-9d38-0309449061df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1bba028-b4e3-4312-95be-c50c09c2f795");
        }
    }
}
