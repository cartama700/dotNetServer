using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dao;
using ServerLib.Utill;

namespace API.Di
{
    /// <summary>
    /// 사용되는 종속성 주입 통합
    /// </summary>
    public class TotalDi
    {
        /// <summary>
        /// 플레이어 정보를 관리
        /// </summary>
        public readonly PlayerDi _playerDi;

        /// <summary>
        /// dbContext 
        /// </summary>
        public readonly MysqlDbContext _mysqlDbContext;

        /// <summary>
        /// dao 종속성
        /// </summary>
        public readonly DaoContext _daoContext;

        /// <summary>
        /// 싱글톤으로 되어있는 마스터 크래스
        /// </summary>
        public readonly MasterCacheUtill _masterCache;

        public TotalDi(
            PlayerDi playerDi,
            MysqlDbContext mysqlDbContext,
            DaoContext daoContext
        )
        {
            _playerDi = playerDi;
            _mysqlDbContext = mysqlDbContext;
            _daoContext = daoContext;
            _masterCache = MasterCacheUtill.GetInstance();
        }
    }
}
