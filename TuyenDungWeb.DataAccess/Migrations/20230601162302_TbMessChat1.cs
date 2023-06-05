using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TbMessChat1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageChat_AspNetUsers_FromUserId",
                table: "MessageChat");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChat_Room_ToRoomId",
                table: "MessageChat");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_AspNetUsers_AdminId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageChat",
                table: "MessageChat");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "MessageChat",
                newName: "MessageChats");

            migrationBuilder.RenameIndex(
                name: "IX_Room_AdminId",
                table: "Rooms",
                newName: "IX_Rooms_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChat_ToRoomId",
                table: "MessageChats",
                newName: "IX_MessageChats_ToRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChat_FromUserId",
                table: "MessageChats",
                newName: "IX_MessageChats_FromUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageChats",
                table: "MessageChats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChats_AspNetUsers_FromUserId",
                table: "MessageChats",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChats_Rooms_ToRoomId",
                table: "MessageChats",
                column: "ToRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_AdminId",
                table: "Rooms",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageChats_AspNetUsers_FromUserId",
                table: "MessageChats");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChats_Rooms_ToRoomId",
                table: "MessageChats");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_AdminId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageChats",
                table: "MessageChats");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "MessageChats",
                newName: "MessageChat");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_AdminId",
                table: "Room",
                newName: "IX_Room_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChats_ToRoomId",
                table: "MessageChat",
                newName: "IX_MessageChat_ToRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChats_FromUserId",
                table: "MessageChat",
                newName: "IX_MessageChat_FromUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageChat",
                table: "MessageChat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChat_AspNetUsers_FromUserId",
                table: "MessageChat",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChat_Room_ToRoomId",
                table: "MessageChat",
                column: "ToRoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_AspNetUsers_AdminId",
                table: "Room",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
