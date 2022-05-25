using Share.Structure;
using System.Collections.Generic;

namespace Share.Protocol.API.Item.Get
{
    /// <summary>
    /// 아이템 타입으로 정보 가져오기
    /// </summary>
    public record ItemGetByTypeProtocol
    {
        public record Response
        {
            public List<ItemStructure> ItemDataList { get; init; } = new List<ItemStructure>();
        }
    }
}
