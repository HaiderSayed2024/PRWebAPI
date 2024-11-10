using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdedcolumnEmalConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "tblUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "tblUsers");
        }
    }
}
