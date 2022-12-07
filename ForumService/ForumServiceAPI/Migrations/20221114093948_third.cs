using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "adminId",
                table: "Forums",
                newName: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Forums",
                newName: "adminId");
        }
    }
}
