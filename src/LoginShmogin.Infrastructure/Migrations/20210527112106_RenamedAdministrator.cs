using Microsoft.EntityFrameworkCore.Migrations;

namespace LoginShmogin.Infrastructure.Migrations
{
    public partial class RenamedAdministrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "73845ee5-7347-4e78-ab9f-d9a7efd398a4", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "558a67ff-4bfd-4b2f-a324-513da62bd393", "AQAAAAEAACcQAAAAEIWc9ZXyuwrv93tDFn7OlR+yytLyVn0AnDSBlDHdRKQylC9vq999eVW6fZPLNWHZ1w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d0341e8-928e-462e-8349-9172d8c919ef",
                column: "ConcurrencyStamp",
                value: "5988c60d-1feb-4bf8-a800-e8060cabdf6e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "808ecb6e-857f-477e-afff-90b844e647af",
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a20c00a4-8f63-4064-aaba-0e43cd0c101e", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1715f36-1e2a-49ef-affe-ca426f97eddc", "AQAAAAEAACcQAAAAEBh/hGLr+CE+OIv0Rlkek99FYtD472M/fmp5bvVVTsHBgob+XGHN6fhrqhXJPbCE7A==" });
        }
    }
}
