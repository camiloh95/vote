using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Web.Migrations
{
    public partial class addVotesResultToCandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VotesResult",
                table: "Candidates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotesResult",
                table: "Candidates");
        }
    }
}
