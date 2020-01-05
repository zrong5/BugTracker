using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackerData.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosedById",
                table: "Bug",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Bug",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    BelongsToTeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Team_BelongsToTeamId",
                        column: x => x.BelongsToTeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bug_ClosedById",
                table: "Bug",
                column: "ClosedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bug_CreatedById",
                table: "Bug",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_BelongsToTeamId",
                table: "ApplicationUser",
                column: "BelongsToTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bug_ApplicationUser_ClosedById",
                table: "Bug",
                column: "ClosedById",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bug_ApplicationUser_CreatedById",
                table: "Bug",
                column: "CreatedById",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bug_ApplicationUser_ClosedById",
                table: "Bug");

            migrationBuilder.DropForeignKey(
                name: "FK_Bug_ApplicationUser_CreatedById",
                table: "Bug");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_Bug_ClosedById",
                table: "Bug");

            migrationBuilder.DropIndex(
                name: "IX_Bug_CreatedById",
                table: "Bug");

            migrationBuilder.DropColumn(
                name: "ClosedById",
                table: "Bug");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Bug");
        }
    }
}
