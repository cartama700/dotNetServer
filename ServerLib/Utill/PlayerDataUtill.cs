using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLib.Utill
{
    public static class PlayerDataUtill
    {
        public static long? GetPlayerId(IHeaderDictionary headers)
        {
            if (headers.TryGetValue("IsNewUser", out _))
            {
                return null;
            }

            if (!headers.TryGetValue("HeaderPlayerId", out var headerPlayerId))
            {
                throw new ArgumentNullException("HeaderPlayerId is NULL");
            }

            if (!long.TryParse(headerPlayerId, out var playerId))
            {
                throw new ArgumentException("PlayerId does not match format");
            }

            return playerId;
        }
    }
}
