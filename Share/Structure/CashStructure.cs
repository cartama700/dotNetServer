using Share.Type.Item;

namespace Share.Structure
{
    /// <summary>
    /// 캐쉬 정보 구조
    /// </summary>
    public record CashStructure
    {
        /// <summary>
        /// <see cref="Enums.Item.CashType">아이템 종류</see> 
        /// </summary>
        public CashType CashType { get; set; }

        /// <summary>
        /// 보유 중인 재화
        /// </summary>
        public uint Count { get; set; }
    }
}
