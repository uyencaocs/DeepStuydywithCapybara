using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeeplearningwithCapybara.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityUserIdToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Users");
        }
    }
}
