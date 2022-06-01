using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;

namespace ServerLib.Database.Mysql.Dao
{

    public class UserItemDao
    {
        protected MysqlDbContext _mysqlDbContext;

        public UserItemDao(MysqlDbContext mysqlDbContext)
        {
            _mysqlDbContext = mysqlDbContext;
        }

        public async Task<UserItemDto> InsertOrUpdateAsync(UserItemDto entity, ushort slotNum)
        {
            var userItemDto = await _mysqlDbContext.UserItemDtos
                .Where(x => x.PlayerId == entity.PlayerId)
                .Where(x => x.ItemId == entity.ItemId)
                .SingleOrDefaultAsync();

            if (userItemDto == null)
            {
                userItemDto = entity;
                userItemDto.Slot = slotNum;
            }
            else
            {
                userItemDto.Count += entity.Count;
                userItemDto.UpdateTime = DateTime.Now;
            }

            _mysqlDbContext.UserItemDtos.Update(userItemDto);

            return userItemDto;
        }

        public void Update(UserItemDto entity)
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
