using Share.Type.Item;

namespace Share.Master.Item
{
    public interface IItemDataMaster : IMasterBase
    {
        /// <summary>
        /// <see cref="Enums.Item.ItemType">아이템 종류</see> 
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// <see cref="Enums.Item.CashType">재화 종류</see> 
        /// </summary>
        public CashType CashType { get; set; }

        /// <summary>
        /// 적용시 사용되는 값
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// 가격
        /// </summary>
        public uint Price { get; set; }
    }
}
