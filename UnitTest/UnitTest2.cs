using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTest
{
    public class AbstractDataExport
    {
        public long testId { get; set; }
    }

    public class TestClass : AbstractDataExport
    {
        public long testId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    public class Tests2
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var itemIds = new List<uint>() { 1, 3 };
            using (var context = new MysqlDbContext())
            {
                var result = await context.UserItemDtos
                    .Where(x => x.PlayerId == 1)
                    .Where(x => x.Slot == 1)
                    .Select(x => new UserItemDto
                    {
                        Id = x.Id,
                        ItemId = x.ItemId,
                        Count = x.Count
                    }).ToListAsync();
                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }


        [Test]
        public async Task test2()
        {

            using (var context = new MysqlDbContext())
            {
                var userItemDtoList = new List<UserItemDto>();

                var testNum = 6;
                var newData = new UserItemDto
                {
                    PlayerId = 1,
                    Slot = (ushort)testNum,
                    ItemId = (uint)testNum,
                    Count = 3,
                };
                var result = context.Add(newData);

                userItemDtoList.Add(result.Entity);
                Console.WriteLine(JsonSerializer.Serialize(userItemDtoList));

                context.SaveChanges();

                Console.WriteLine(userItemDtoList.Count());
                Console.WriteLine(JsonSerializer.Serialize(userItemDtoList));
            }
        }

        [Test]
        public async Task test3()
        {

        }
    }
}