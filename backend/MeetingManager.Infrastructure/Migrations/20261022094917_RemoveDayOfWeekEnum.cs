using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDayOfWeekEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:attendance_status", "pending,accepted,declined")
                .OldAnnotation("Npgsql:Enum:attendance_status", "pending,accepted,declined")
                .OldAnnotation("Npgsql:Enum:day_of_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:attendance_status", "pending,accepted,declined")
                .Annotation("Npgsql:Enum:day_of_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .OldAnnotation("Npgsql:Enum:attendance_status", "pending,accepted,declined");
        }
    }
}
