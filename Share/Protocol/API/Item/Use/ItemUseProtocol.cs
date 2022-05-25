using Share.Structure;
using System.Collections.Generic;

namespace Share.Protocol.API.Item.Use
{
    public record ItemUseProtocol
    {
        public record Request
        {
            public List<ItemStructure> ItemDataList { get; init; } = new List<ItemStructure>();
        }

        public record Response
        {
            public List<ItemStructure> ItemDataList { get; init; } = new List<ItemStructure>();
        }
    }
}
