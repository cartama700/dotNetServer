using Share.Type.Item;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Share.Master.Item;

namespace ServerLib.Database.Mysql.Dto.Master.Item
{
    [Table("master_item")]
    public class MasterItemDto : IItemDataMaster
    {
        [Key, Column(Order = 0)]
        [Comment("아이디")]
        public uint Id { get; set; }

        [Column(Order = 1)]
        [Comment("이름")]
        public string Name { get; set; }

        [Column(Order = 2)]
        [Comment("사용 여부")]
        public bool IsUse { get; set; }

        [Column(Order = 3)]
        [Comment("아이템 종류")]
        public ItemType ItemType { get; set; }

        [Column(Order = 4)]
        [Comment("캐쉬 종류")]
        public CashType CashType { get; set; }

        [Column(Order = 5)]
        [Comment("적용 값")]
        public uint Value { get; set; }

        [Column(Order = 6)]
        [Comment("가격")]
        public uint Price { get; set; }
    }
}
