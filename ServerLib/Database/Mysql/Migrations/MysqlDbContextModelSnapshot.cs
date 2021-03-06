// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerLib.Database.Mysql.Context;

#nullable disable

namespace ServerLib.Database.Mysql.Migrations
{
    [DbContext(typeof(MysqlDbContext))]
    partial class MysqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ServerLib.Database.Mysql.Dto.Master.Item.MasterItemDto", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(0)
                        .HasComment("아이디");

                    b.Property<int>("CashType")
                        .HasColumnType("int")
                        .HasColumnOrder(4)
                        .HasComment("캐쉬 종류");

                    b.Property<bool>("IsUse")
                        .HasColumnType("tinyint(1)")
                        .HasColumnOrder(2)
                        .HasComment("사용 여부");

                    b.Property<int>("ItemType")
                        .HasColumnType("int")
                        .HasColumnOrder(3)
                        .HasComment("아이템 종류");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnOrder(1)
                        .HasComment("이름");

                    b.Property<uint>("Price")
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(6)
                        .HasComment("가격");

                    b.Property<uint>("Value")
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(5)
                        .HasComment("적용 값");

                    b.HasKey("Id");

                    b.ToTable("master_item");
                });

            modelBuilder.Entity("ServerLib.Database.Mysql.Dto.User.UserCashDto", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    b.Property<int>("CashType")
                        .HasColumnType("int")
                        .HasColumnOrder(2)
                        .HasComment("캐쉬 타입");

                    b.Property<uint>("Count")
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(3)
                        .HasComment("소지 수");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(1)
                        .HasComment("플레이어 아이디");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PlayerId", "CashType" }, "UNQ_PlayerId_CashType")
                        .IsUnique();

                    b.ToTable("user_cash");

                    b.HasComment("유저의 캐쉬 보유 현황");
                });

            modelBuilder.Entity("ServerLib.Database.Mysql.Dto.User.UserDataDto", b =>
                {
                    b.Property<long>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0)
                        .HasComment("플레이어 아이디");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("PlayerId");

                    b.HasIndex(new[] { "PlayerName" }, "UNQ_PlayerName")
                        .IsUnique();

                    b.ToTable("user_data");

                    b.HasComment("유저의 기본 데이터");
                });

            modelBuilder.Entity("ServerLib.Database.Mysql.Dto.User.UserItemDto", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    b.Property<uint>("Count")
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(4)
                        .HasComment("아이템 보유 수");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<uint>("ItemId")
                        .HasColumnType("int unsigned")
                        .HasColumnOrder(3)
                        .HasComment("아이템 고유 아이디");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(1)
                        .HasComment("플레이어 아이디");

                    b.Property<ushort>("Slot")
                        .HasColumnType("smallint unsigned")
                        .HasColumnOrder(2)
                        .HasComment("아이템 슬롯");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ItemId" }, "UNQ_ItemId")
                        .IsUnique();

                    b.HasIndex(new[] { "PlayerId", "Slot" }, "UNQ_PlayerId_Slot")
                        .IsUnique();

                    b.ToTable("user_item");

                    b.HasComment("유저의 아이템 소지 정보");
                });
#pragma warning restore 612, 618
        }
    }
}
