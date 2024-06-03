using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOOK.DB.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_SysUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SysUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Borrowed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UID = table.Column<int>(type: "int", nullable: false),
                    BID = table.Column<int>(type: "int", nullable: false),
                    BorrowedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Borrowed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Borrowed_TB_Book_BID",
                        column: x => x.BID,
                        principalTable: "TB_Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Borrowed_TB_SysUser_UID",
                        column: x => x.UID,
                        principalTable: "TB_SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_SysUser_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UID = table.Column<int>(type: "int", nullable: false),
                    RID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SysUser_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_SysUser_Role_TB_Role_RID",
                        column: x => x.RID,
                        principalTable: "TB_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_SysUser_Role_TB_SysUser_UID",
                        column: x => x.UID,
                        principalTable: "TB_SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Borrowed_BID",
                table: "TB_Borrowed",
                column: "BID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Borrowed_UID",
                table: "TB_Borrowed",
                column: "UID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SysUser_Role_RID",
                table: "TB_SysUser_Role",
                column: "RID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SysUser_Role_UID",
                table: "TB_SysUser_Role",
                column: "UID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Borrowed");

            migrationBuilder.DropTable(
                name: "TB_SysUser_Role");

            migrationBuilder.DropTable(
                name: "TB_Book");

            migrationBuilder.DropTable(
                name: "TB_Role");

            migrationBuilder.DropTable(
                name: "TB_SysUser");
        }
    }
}
