using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.Master.Item;

namespace ServerLib.Utill
{
    /// <summary>
    /// 마스터 캐쉬
    /// </summary>
    public class MasterCacheUtill
    {
        private static MasterCacheUtill? _instance = null;

        private Dictionary<string, SortedList<uint, object>> _masterCache = new Dictionary<string, SortedList<uint, object>>();

        private MasterCacheUtill()
        {
        }

        public static MasterCacheUtill GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MasterCacheUtill();
            }

            return _instance;
        }

        /// <summary>
        /// DB에서 마스터 정보를 가져와서 저장
        /// 리플렉션으로 자동 반영 수정 예정
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            using var context = new MysqlDbContext();
            var masterItemDtoList = await context.MasterItemDtos.ToListAsync();

            var dtoName = nameof(MasterItemDto);
            foreach (var masterItemDto in masterItemDtoList)
            {
                if (!masterItemDto.IsUse)
                {
                    continue;
                }

                if (!_masterCache.TryGetValue(dtoName, out var masterData))
                {
                    _masterCache.Add(dtoName, new SortedList<uint, object>());
                    masterData = _masterCache[dtoName];
                }

                masterData.Add(masterItemDto.Id, masterItemDto);
            }
        }

        public T Get<T>(uint id) where T : class
        {
            if (!_masterCache.TryGetValue(typeof(T).Name, out var masterData))
            {
                throw new Exception("없는 마스터 테이블 입니다.");
            }

            if (!masterData.TryGetValue(id, out var result))
            {
                throw new ArgumentException("없는 마스터 아이디 입니다.");
            }

            var value = result as T;
            if (value == null)
            {
                throw new ArgumentException("없는 마스터 아이디 입니다.");
            }

            return value;
        }

        public List<T> Get<T>(List<uint> ids) where T : class
        {
            if (!_masterCache.TryGetValue(typeof(T).Name, out var masterData))
            {
                throw new Exception("없는 마스터 테이블 입니다.");
            }

            var result = masterData.Where(x => ids.Contains(x.Key)).Select(x => x.Value).ToList();

            if (result.Count != ids.Count)
            {
                throw new ArgumentException("없는 마스터 아이디 입니다.");
            }

            return result.Select(x =>
            {
                var value = x as T;
                if (value == null)
                {
                    throw new Exception("맞지않는 형식입니다.");
                }

                return value;
            }).ToList();
        }

        public List<T> All<T>() where T : class
        {
            if (!_masterCache.TryGetValue(typeof(T).Name, out var masterData))
            {
                throw new Exception("없는 마스터 테이블 입니다.");
            }

            return masterData.Select(x =>
            {
                var value = x.Value as T;
                if (value == null)
                {
                    throw new Exception("맞지않는 형식입니다.");
                }

                return value;
            }).ToList();
        }
    }
}
