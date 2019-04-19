using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Web.Migrations
{
    public partial class changingVoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "voteResult",
                table: "Candidates",
                newName: "VoteResult");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Votes",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoteResult",
                table: "Candidates",
                newName: "voteResult");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Votes",
                nullable: false,
                oldClrType: typeof(Guid));
        }
    }
}
