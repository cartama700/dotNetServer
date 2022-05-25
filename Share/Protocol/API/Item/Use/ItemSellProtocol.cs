using Share.Structure;
using System.Collections.Generic;

namespace Share.Protocol.API.Item.Use
{
    public record ItemSellProtocol
    {
        public record Request
        {
            public List<ItemStructure> ItemStructureList { get; init; } = new List<ItemStructure>();
        }

        public record Response
        {
            public List<CashStructure> CashStructuresList { get; init; } = new List<CashStructure>();
        }
    }
}
