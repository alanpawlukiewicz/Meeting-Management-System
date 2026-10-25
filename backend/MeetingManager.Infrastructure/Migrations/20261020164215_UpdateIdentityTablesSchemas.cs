using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityTablesSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_meeting_participants_user_user_id",
                table: "meeting_participants");

            migrationBuilder.DropForeignKey(
                name: "fk_meetings_user_creator_id",
                table: "meetings");

            migrationBuilder.DropForeignKey(
                name: "fk_user_availabilities_user_user_id",
                table: "user_availabilities");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id1",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id1",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "ix_user_roles_role_id1",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "ix_user_roles_user_id1",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "lockout_enabled",
                table: "users");

            migrationBuilder.DropColumn(
                name: "role_id1",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "normalized_name",
                table: "roles");

            migrationBuilder.RenameColumn(
                name: "user_id1",
                table: "user_roles",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                table: "users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "roles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_meeting_participants_users_user_id",
                table: "meeting_participants",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_meetings_users_creator_id",
                table: "meetings",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_availabilities_users_user_id",
                table: "user_availabilities",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_meeting_participants_users_user_id",
                table: "meeting_participants");

            migrationBuilder.DropForeignKey(
                name: "fk_meetings_users_creator_id",
                table: "meetings");

            migrationBuilder.DropForeignKey(
                name: "fk_user_availabilities_users_user_id",
                table: "user_availabilities");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_roles",
                newName: "user_id1");

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                table: "users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<bool>(
                name: "lockout_enabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "role_id1",
                table: "user_roles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "roles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "concurrency_stamp",
                table: "roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "normalized_name",
                table: "roles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id1",
                table: "user_roles",
                column: "role_id1");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id1",
                table: "user_roles",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_meeting_participants_user_user_id",
                table: "meeting_participants",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_meetings_user_creator_id",
                table: "meetings",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_availabilities_user_user_id",
                table: "user_availabilities",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id1",
                table: "user_roles",
                column: "role_id1",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id1",
                table: "user_roles",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
