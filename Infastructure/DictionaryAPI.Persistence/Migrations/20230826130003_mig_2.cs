using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryVotes",
                table: "EntryVotes");

            migrationBuilder.DropIndex(
                name: "IX_EntryVotes_UserId",
                table: "EntryVotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryVotes",
                table: "EntryVotes",
                columns: new[] { "UserId", "EntryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryVotes",
                table: "EntryVotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryVotes",
                table: "EntryVotes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EntryVotes_UserId",
                table: "EntryVotes",
                column: "UserId");
        }
    }
}
