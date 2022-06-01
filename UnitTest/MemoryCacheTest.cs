using NUnit.Framework;
using ServerLib.Database.Mysql.Dto.User;
using Share.Type.Item;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTest
{
    public class MemoryCacheTest
    {

        [Test]
        public async Task Test1()
        {
            var ht = new HashSet<UserDataDto>();
            ht.Add(new UserDataDto { PlayerId = 1, PlayerName = "12", CreateTime = DateTime.Now, UpdateTime = DateTime.Now });

            var test1 = new MemoryCache("test1");
            var test2 = new MemoryCache("test1");
            test1.Set("1", new UserDataDto { PlayerId = 1, PlayerName = "12", CreateTime = DateTime.Now, UpdateTime = DateTime.Now }, DateTimeOffset.MaxValue);
            test2.Set("1", new UserCashDto { PlayerId = 1, CashType = CashType.Gold, Count = 1, Id = 1 }, DateTimeOffset.MaxValue);

            var userData = test1.Get("1");
            var userCashDto = test2.Get("1");
            Console.WriteLine(JsonSerializer.Serialize(userData));
            Console.WriteLine(JsonSerializer.Serialize(userCashDto));
        }

        [Test]
        public void Test2()
        {
            uint a = 0;
            try
            {

                var s = a - 1;
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
