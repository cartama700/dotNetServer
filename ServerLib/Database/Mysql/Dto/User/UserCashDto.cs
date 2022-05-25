using Microsoft.EntityFrameworkCore;
using Share.Type.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Share.Structure;

namespace ServerLib.Database.Mysql.Dto.User
{
    [Table("user_cash")]
    [Comment("유저의 캐쉬 보유 현황")]
    [Index(nameof(PlayerId), nameof(CashType), IsUnique = true, Name = "UNQ_PlayerId_CashType")]
    public record UserCashDto : BaseDto, IUserBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }

        [Column(Order = 1)]
        [Comment("플레이어 아이디")]
        public long PlayerId { get; set; }

        [Column(Order = 2)]
        [Comment("캐쉬 타입")]
        public CashType CashType { get; set; }


        [Column(Order = 3)]
        [Comment("소지 수")]
        public uint Count { get; set; }

        public CashStructure ToCashStructure()
        {
            return new CashStructure
            {
                CashType = CashType,
                Count = Count
            };
        }
    }
}
