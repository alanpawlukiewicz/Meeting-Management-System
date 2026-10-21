using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO roles VALUES ('890af062-d337-436c-a109-60196abd3ef6', 'Admin');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM roles WHERE name LIKE 'Admin';");
        }
    }
}
