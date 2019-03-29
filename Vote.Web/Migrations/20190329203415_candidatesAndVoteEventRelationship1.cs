using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Web.Migrations
{
    public partial class candidatesAndVoteEventRelationship1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_VoteEvents_VoteEventId",
                table: "Candidate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate");

            migrationBuilder.RenameTable(
                name: "Candidate",
                newName: "Candidates");

            migrationBuilder.RenameIndex(
                name: "IX_Candidate_VoteEventId",
                table: "Candidates",
                newName: "IX_Candidates_VoteEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidates",
                table: "Candidates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_VoteEvents_VoteEventId",
                table: "Candidates",
                column: "VoteEventId",
                principalTable: "VoteEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_VoteEvents_VoteEventId",
                table: "Candidates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidates",
                table: "Candidates");

            migrationBuilder.RenameTable(
                name: "Candidates",
                newName: "Candidate");

            migrationBuilder.RenameIndex(
                name: "IX_Candidates_VoteEventId",
                table: "Candidate",
                newName: "IX_Candidate_VoteEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_VoteEvents_VoteEventId",
                table: "Candidate",
                column: "VoteEventId",
                principalTable: "VoteEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
