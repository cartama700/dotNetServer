using Share.Structure;
using System.Collections.Generic;

namespace Share.Protocol.API.Item.Buy
{
    public record ItemBuyProtocol
    {
        public record Request
        {
            public List<ItemStructure> ItemStructureList { get; init; } = new List<ItemStructure>();
        }

        public record Response
        {
            public List<CashStructure> CashStructureList { get; init; } = new List<CashStructure>();

            public List<ItemStructure> ItemStructureList { get; init; } = new List<ItemStructure>();
        }
    }
}
