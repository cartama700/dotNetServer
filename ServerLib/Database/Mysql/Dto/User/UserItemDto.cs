using Microsoft.EntityFrameworkCore;
using Share.Structure;
using Share.Type.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerLib.Database.Mysql.Dto.User
{
    [Table("user_item")]
    [Comment("유저의 아이템 소지 정보")]
    [Index(nameof(PlayerId), nameof(Slot), IsUnique = true, Name = "UNQ_PlayerId_Slot")]
    [Index(nameof(ItemId), IsUnique = true, Name = "UNQ_ItemId")]
    public record UserItemDto : BaseDto, IUserBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }

        [Column(Order = 1)]
        [Comment("플레이어 아이디")]
        public long PlayerId { get; set; }

        [Column(Order = 2)]
        [Comment("아이템 슬롯")]
        public ushort Slot { get; set; }

        [Column(Order = 3)]
        [Comment("아이템 고유 아이디")]
        public uint ItemId { get; set; }

        [Column(Order = 4)]
        [Comment("아이템 보유 수")]
        public uint Count { get; set; }

        public ItemStructure ToItemStructure(ItemType type)
        {
            return new ItemStructure
            {
                Id = ItemId,
                Type = type,
                Count = Count
            };
        }
    }
}
