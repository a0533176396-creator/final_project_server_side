using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "ContentURL");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Role",
            //    table: "Messages",
            //    type: "integer",
            //    maxLength: 50,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "character varying(50)",
            //    oldMaxLength: 50);
            // 1. מחיקת העמודה הישנה
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Messages");

            // 2. יצירת העמודה החדשה כ-Integer
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentURL",
                table: "Messages",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Messages",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 50);
        }
    }
}
