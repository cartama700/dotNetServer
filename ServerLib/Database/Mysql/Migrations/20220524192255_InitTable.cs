using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLib.Database.Mysql.Migrations
{
    public partial class InitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "master_item",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false, comment: "아이디")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "이름")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUse = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "사용 여부"),
                    ItemType = table.Column<int>(type: "int", nullable: false, comment: "아이템 종류"),
                    CashType = table.Column<int>(type: "int", nullable: false, comment: "캐쉬 종류"),
                    Value = table.Column<uint>(type: "int unsigned", nullable: false, comment: "적용 값"),
                    Price = table.Column<uint>(type: "int unsigned", nullable: false, comment: "가격")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_item", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_cash",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false, comment: "플레이어 아이디"),
                    CashType = table.Column<int>(type: "int", nullable: false, comment: "캐쉬 타입"),
                    Count = table.Column<uint>(type: "int unsigned", nullable: false, comment: "소지 수"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_cash", x => x.Id);
                },
                comment: "유저의 캐쉬 보유 현황")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_data",
                columns: table => new
                {
                    PlayerId = table.Column<long>(type: "bigint", nullable: false, comment: "플레이어 아이디")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerName = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_data", x => x.PlayerId);
                },
                comment: "유저의 기본 데이터")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false, comment: "플레이어 아이디"),
                    Slot = table.Column<ushort>(type: "smallint unsigned", nullable: false, comment: "아이템 슬롯"),
                    ItemId = table.Column<uint>(type: "int unsigned", nullable: false, comment: "아이템 고유 아이디"),
                    Count = table.Column<uint>(type: "int unsigned", nullable: false, comment: "아이템 보유 수"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_item", x => x.Id);
                },
                comment: "유저의 아이템 소지 정보")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "UNQ_PlayerId_CashType",
                table: "user_cash",
                columns: new[] { "PlayerId", "CashType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UNQ_PlayerName",
                table: "user_data",
                column: "PlayerName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UNQ_ItemId",
                table: "user_item",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UNQ_PlayerId_Slot",
                table: "user_item",
                columns: new[] { "PlayerId", "Slot" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "master_item");

            migrationBuilder.DropTable(
                name: "user_cash");

            migrationBuilder.DropTable(
                name: "user_data");

            migrationBuilder.DropTable(
                name: "user_item");
        }
    }
}
