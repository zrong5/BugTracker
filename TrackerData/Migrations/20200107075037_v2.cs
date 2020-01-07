using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToId",
                table: "Bug",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bug_AssignedToId",
                table: "Bug",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bug_AspNetUsers_AssignedToId",
                table: "Bug",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bug_AspNetUsers_AssignedToId",
                table: "Bug");

            migrationBuilder.DropIndex(
                name: "IX_Bug_AssignedToId",
                table: "Bug");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Bug");
        }
    }
}
