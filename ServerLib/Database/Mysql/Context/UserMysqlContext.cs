using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.User;

namespace ServerLib.Database.Mysql.Context
{
    /// <summary>
    /// 유저 DbContext
    /// </summary>
    public partial class MysqlDbContext : DbContext
    {
        #region DbSet

        public DbSet<UserItemDto> UserItemDtos { get; set; }

        public DbSet<UserDataDto> UserDataDtos { get; set; }

        public DbSet<UserCashDto> UserCashDtos { get; set; }

        #endregion

        protected void UserOnModelCreating(ModelBuilder modelBuilder)
        {
            #region Entity

            modelBuilder.Entity<UserItemDto>();

            modelBuilder.Entity<UserDataDto>();

            modelBuilder.Entity<UserCashDto>();

            #endregion
        }
    }
}
