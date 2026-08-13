using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Baseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.CreateTable(
        //        name: "Categories",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        //            Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
        //            father_id = table.Column<int>(type: "integer", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Categories", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Categories_Categories_father_id",
        //                column: x => x.father_id,
        //                principalTable: "Categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Users",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            sub = table.Column<string>(type: "text", nullable: false),
        //            First_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        //            Last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        //            Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
        //            Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
        //            Wont_help = table.Column<bool>(type: "boolean", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Users", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ChatSessions",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            UserId = table.Column<int>(type: "integer", nullable: false),
        //            Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
        //            CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
        //            UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ChatSessions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ChatSessions_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "FavoriteUserCategories",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            user_id = table.Column<int>(type: "integer", nullable: false),
        //            category_id = table.Column<int>(type: "integer", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_FavoriteUserCategories", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_FavoriteUserCategories_Categories_category_id",
        //                column: x => x.category_id,
        //                principalTable: "Categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_FavoriteUserCategories_Users_user_id",
        //                column: x => x.user_id,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Tasks",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
        //            Task_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
        //            user_id = table.Column<int>(type: "integer", nullable: false),
        //            CategoryId = table.Column<int>(type: "integer", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Tasks", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Tasks_Categories_CategoryId",
        //                column: x => x.CategoryId,
        //                principalTable: "Categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_Tasks_Users_user_id",
        //                column: x => x.user_id,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Messages",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            SessionId = table.Column<int>(type: "integer", nullable: false),
        //            Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
        //            Content = table.Column<string>(type: "text", nullable: false),
        //            CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Messages", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Messages_ChatSessions_SessionId",
        //                column: x => x.SessionId,
        //                principalTable: "ChatSessions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "taskfiles",
        //        columns: table => new
        //        {
        //            fileid = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            taskid = table.Column<int>(type: "integer", nullable: false),
        //            filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
        //            fileurl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
        //            uploaddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_taskfiles", x => x.fileid);
        //            table.ForeignKey(
        //                name: "FK_taskfiles_Tasks_taskid",
        //                column: x => x.taskid,
        //                principalTable: "Tasks",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Categories_father_id",
        //        table: "Categories",
        //        column: "father_id");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ChatSessions_UserId",
        //        table: "ChatSessions",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_FavoriteUserCategories_category_id",
        //        table: "FavoriteUserCategories",
        //        column: "category_id");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_FavoriteUserCategories_user_id_category_id",
        //        table: "FavoriteUserCategories",
        //        columns: new[] { "user_id", "category_id" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Messages_SessionId",
        //        table: "Messages",
        //        column: "SessionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_taskfiles_taskid",
        //        table: "taskfiles",
        //        column: "taskid");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Tasks_CategoryId",
        //        table: "Tasks",
        //        column: "CategoryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Tasks_user_id",
        //        table: "Tasks",
        //        column: "user_id");
        //}

        ///// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "FavoriteUserCategories");

        //    migrationBuilder.DropTable(
        //        name: "Messages");

        //    migrationBuilder.DropTable(
        //        name: "taskfiles");

        //    migrationBuilder.DropTable(
        //        name: "ChatSessions");

        //    migrationBuilder.DropTable(
        //        name: "Tasks");

        //    migrationBuilder.DropTable(
        //        name: "Categories");

        //    migrationBuilder.DropTable(
        //        name: "Users");
        }
    }
}
