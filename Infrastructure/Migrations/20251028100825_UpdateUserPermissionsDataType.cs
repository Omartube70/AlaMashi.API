using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPermissionsDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Permissions",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserPermissions",
                table: "Users",
                type: "Nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserPermissions",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "Nvarchar(50)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Permissions",
                table: "Users",
                sql: "[UserPermissions] IN (1, 2)");
        }
    }
}
