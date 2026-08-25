using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTree.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTokenHashFielName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashToken",
                table: "RefreshTokens",
                newName: "TokenHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "RefreshTokens",
                newName: "HashToken");
        }
    }
}
