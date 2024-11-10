using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserTableAddedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "tblUsers",
                newName: "Password");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "tblUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tblUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "tblUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "tblUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "tblUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "tblUsers");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "tblUsers",
                newName: "UserName");
        }
    }
}
