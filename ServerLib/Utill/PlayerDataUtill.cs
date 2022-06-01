using Microsoft.AspNetCore.Http;

namespace ServerLib.Utill
{
    /// <summary>
    /// 플레이어 데이터 유틸
    /// </summary>
    public static class PlayerDataUtill
    {
        /// <summary>
        /// 헤더에서 플레이어 아이디를 가져옴
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
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
