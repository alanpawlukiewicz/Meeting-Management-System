using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_user_time_offs_user_id",
                table: "user_time_offs",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_time_offs_users_user_id",
                table: "user_time_offs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("INSERT INTO public.users(id, first_name, last_name, user_name, password_hash, lockout_end, access_failed_count) VALUES ('fdaa91d6-9ad8-4821-97ca-be9905295bc4', null, null, 'Admin', '$2a$11$R2JLidV15rslMM8rNxr.TOE05Xc5cuzLJFSOAn85HBM.iDjIrv9Mu', null, 0);");
            migrationBuilder.Sql("INSERT INTO public.user_roles(user_id, role_id, id) VALUES ('fdaa91d6-9ad8-4821-97ca-be9905295bc4', '3d2ca1e0-a716-4a8c-901e-9b0bfd563084', 'b971a4a9-f189-453c-ae08-ade9296428ff'), ('fdaa91d6-9ad8-4821-97ca-be9905295bc4', '890af062-d337-436c-a109-60196abd3ef6', '385d8dcc-e135-41b8-839e-a416647bfc7b');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_time_offs_users_user_id",
                table: "user_time_offs");

            migrationBuilder.DropIndex(
                name: "ix_user_time_offs_user_id",
                table: "user_time_offs");

            migrationBuilder.Sql("DELETE FROM public.user_roles WHERE id IN ('b971a4a9-f189-453c-ae08-ade9296428ff', '385d8dcc-e135-41b8-839e-a416647bfc7b');");
            migrationBuilder.Sql("DELETE FROM public.users WHERE id = 'fdaa91d6-9ad8-4821-97ca-be9905295bc4';");
        }
    }
}
