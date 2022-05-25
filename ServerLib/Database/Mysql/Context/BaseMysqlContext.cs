using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.User;

namespace ServerLib.Database.Mysql.Context
{
    /// <summary>
    /// 기본 DbContext
    /// </summary>
    public partial class MysqlDbContext : DbContext
    {
        public MysqlDbContext()
        {
        }

        public MysqlDbContext(DbContextOptions<MysqlDbContext> options)
            : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=rootroot!2;database=portfolio", new MySqlServerVersion(new Version(8, 0, 28)));
            optionsBuilder.LogTo(message => Console.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            UserOnModelCreating(modelBuilder);

            MasterOnModelCreating(modelBuilder);
        }
    }
}
