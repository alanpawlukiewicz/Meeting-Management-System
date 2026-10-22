using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO roles VALUES ('3d2ca1e0-a716-4a8c-901e-9b0bfd563084', 'Organizer');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM roles WHERE name LIKE 'Organizer';");
        }
    }
}
