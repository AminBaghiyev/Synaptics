using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synaptics.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PostCommentReplyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "PostComments",
                type: "BIGINT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_ParentId",
                table: "PostComments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_PostComments_ParentId",
                table: "PostComments",
                column: "ParentId",
                principalTable: "PostComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_PostComments_ParentId",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_ParentId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PostComments");
        }
    }
}
