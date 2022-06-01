using Share.Structure;
using System.Collections.Generic;

namespace Share.Protocol.API.Item.Get
{
    /// <summary>
    /// 아이템 아이디로 정보를 요청
    /// </summary>
    public record ItemGetProtocolRequest
    {
        public string Ids { get; init; }
    }

    public record ItemGetProtocolResponse
    {
        public List<ItemStructure> ItemDataList { get; init; } = new List<ItemStructure>();
    }
}
