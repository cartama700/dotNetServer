using Microsoft.AspNetCore.Http;
using Share.Const;
using System;

namespace Share.Utill
{
    public static class PlayerDataUtill
    {
        public static long GetPlayerId(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue(HeaderConst.HeaderPlayerId, out var headerPlayerId))
            {
                throw new ArgumentNullException($"{nameof(HeaderConst.HeaderPlayerId)} is NULL");
            }

            if (!long.TryParse(headerPlayerId, out var playerId))
            {
                throw new ArgumentException($"{nameof(HeaderConst.HeaderPlayerId)} does not match format");
            }

            return playerId;
        }
    }
}
