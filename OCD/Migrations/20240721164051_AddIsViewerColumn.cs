using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OCD.Migrations
{
    /// <inheritdoc />
    public partial class AddIsViewerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isViewer",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isViewer",
                table: "AspNetUsers");
        }
    }
}
