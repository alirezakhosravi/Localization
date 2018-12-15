using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalizationTest.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "LocalizationRecord",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: false),
                    CultureName = table.Column<string>(maxLength: 10, nullable: false),
                    ResourceName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationRecord", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationRecord_Name_CultureName_ResourceName_TenantId",
                schema: "dbo",
                table: "LocalizationRecord",
                columns: new[] { "Name", "CultureName", "ResourceName", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizationRecord",
                schema: "dbo");
        }
    }
}
