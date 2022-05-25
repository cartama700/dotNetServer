using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;

namespace ServerLib.Database.Mysql.Dao
{
    public class UserCashDao : DaoBase<UserCashDto>
    {
        protected MysqlDbContext _mysqlDbContext;

        public UserCashDao(MysqlDbContext mysqlDbContext)
        {
            _mysqlDbContext = mysqlDbContext;
        }

        public async Task<UserCashDto> InsertOrUpdateAsync(UserCashDto entity)
        {
            var userCashDto = await _mysqlDbContext.UserCashDtos
                .Where(x => x.PlayerId == entity.PlayerId)
                .SingleOrDefaultAsync(x => x.CashType == entity.CashType);

            if (userCashDto == null)
            {
                userCashDto = entity;
                await _mysqlDbContext.UserCashDtos.AddAsync(entity);
            }
            else
            {
                userCashDto.Count += entity.Count;
                _mysqlDbContext.Attach(userCashDto);
            }

            return userCashDto;
        }

        public void Update(UserCashDto entity)
        {
            if (entity.Id == 0)
            {
                throw new NotImplementedException("기본 키는 필수 입니다.");
            }

            entity.UpdateTime = DateTime.Now;

            var entry = _mysqlDbContext.Entry(entity);

            entry.Property(x => x.UpdateTime).IsModified = true;
            entry.Property(x => x.Count).IsModified = true;
        }
    }
}
