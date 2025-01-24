using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88705616-7e1f-405f-87b5-627dae83c6b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c426a636-10c0-45bc-a48f-7e11077c5790");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf98dab8-b541-45f0-97b1-1ce785ad2ea4");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "tblInteractionDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "tblInteractionDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57708ae5-7785-4ada-9546-edfa58f8f6f2", "1", "Admin", "Admin" },
                    { "7df74360-e21d-4591-8d0f-82b9fe348c9e", "1", "Admin", "User" },
                    { "8bea4e5e-14cf-451e-a69b-474c1555fc07", "1", "Admin", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57708ae5-7785-4ada-9546-edfa58f8f6f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7df74360-e21d-4591-8d0f-82b9fe348c9e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bea4e5e-14cf-451e-a69b-474c1555fc07");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "tblInteractionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "tblInteractionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88705616-7e1f-405f-87b5-627dae83c6b6", "1", "Admin", "User" },
                    { "c426a636-10c0-45bc-a48f-7e11077c5790", "1", "Admin", "HR" },
                    { "cf98dab8-b541-45f0-97b1-1ce785ad2ea4", "1", "Admin", "Admin" }
                });
        }
    }
}
