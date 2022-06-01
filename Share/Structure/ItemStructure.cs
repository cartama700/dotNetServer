using Share.Type.Item;

namespace Share.Structure
{
    /// <summary>
    /// 아이템 정보 구조
    /// </summary>
    public record ItemStructure
    {
        /// <summary>
        /// <see cref="Enums.Item.ItemType">아이템 종류</see> 
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// 아이템 아이디
        /// </summary>
        public uint Id { get; set; }

        public uint Count { get; set; }
    }
}
