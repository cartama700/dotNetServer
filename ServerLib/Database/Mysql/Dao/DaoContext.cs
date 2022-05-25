using ServerLib.Database.Mysql.Context;

namespace ServerLib.Database.Mysql.Dao
{
    public class DaoContext
    {
        private MysqlDbContext _mysqlDbContext;

        public UserItemDao _userItemDao { get; set; }

        public UserCashDao _userCashDao { get; set; }

        public DaoContext(MysqlDbContext mysqlDbContext)
        {
            _mysqlDbContext = mysqlDbContext;

            _userItemDao = new UserItemDao(_mysqlDbContext);
            _userCashDao = new UserCashDao(_mysqlDbContext);
        }
    }
}
