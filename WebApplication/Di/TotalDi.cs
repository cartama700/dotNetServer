using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dao;

namespace API.Di
{
    /// <summary>
    /// 사용되는 종속성 주입 통합
    /// </summary>
    public class TotalDi
    {
        public readonly PlayerDi _playerDi;

        public readonly MysqlDbContext _mysqlDbContext;

        public readonly DaoContext _daoContext; 

        public TotalDi(
            PlayerDi playerDi,
            MysqlDbContext mysqlDbContext,
            DaoContext daoContext
        )
        {
            _playerDi = playerDi;
            _mysqlDbContext = mysqlDbContext;
            _daoContext = daoContext;
        }
    }
}
