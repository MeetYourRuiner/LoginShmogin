using Microsoft.EntityFrameworkCore.Migrations;

namespace LoginShmogin.Infrastructure.Migrations
{
    public partial class FixedUserRoleConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d0341e8-928e-462e-8349-9172d8c919ef",
                column: "ConcurrencyStamp",
                value: "2210726a-c171-4f18-8c69-aa1def0a0dba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "808ecb6e-857f-477e-afff-90b844e647af",
                column: "ConcurrencyStamp",
                value: "7875acdc-6cc3-47b8-83fc-d50f422dc8e9");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "808ecb6e-857f-477e-afff-90b844e647af", "B22698B8-42A2-4115-9631-1C2D1E2AC5F7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2a9f262f-feb7-41ee-8680-51dd31acee07", "AQAAAAEAACcQAAAAEOrSoHi4SDg935vOdMLqo1WQba9ZUrAkpfmGd6SPSFJIJdnTkFrnHdNl5nb2uHG3Yg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "808ecb6e-857f-477e-afff-90b844e647af", "B22698B8-42A2-4115-9631-1C2D1E2AC5F7" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d0341e8-928e-462e-8349-9172d8c919ef",
                column: "ConcurrencyStamp",
                value: "6e70ed41-ccf9-45bc-b0fa-7a533042308b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "808ecb6e-857f-477e-afff-90b844e647af",
                column: "ConcurrencyStamp",
                value: "73845ee5-7347-4e78-ab9f-d9a7efd398a4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "558a67ff-4bfd-4b2f-a324-513da62bd393", "AQAAAAEAACcQAAAAEIWc9ZXyuwrv93tDFn7OlR+yytLyVn0AnDSBlDHdRKQylC9vq999eVW6fZPLNWHZ1w==" });
        }
    }
}
