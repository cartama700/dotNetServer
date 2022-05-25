using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.Master.Item;

namespace ServerLib.Database.Mysql.Context
{
    /// <summary>
    /// 마스터 DbContext 
    /// </summary>
    public partial class MysqlDbContext : DbContext
    {

        #region 마스터 데이터

        #region 아이템

        public DbSet<MasterItemDto> MasterItemDtos { get; set; }

        #endregion

        #endregion

        protected void MasterOnModelCreating(ModelBuilder modelBuilder)
        {
            #region Entity

            #region 아이템

            modelBuilder.Entity<MasterItemDto>();

            #endregion

            #endregion
        }
    }
}
