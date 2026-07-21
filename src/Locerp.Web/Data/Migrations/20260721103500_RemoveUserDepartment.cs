using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locerp.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
