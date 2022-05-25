using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace UnitTest
{
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
                var result = await context.UserItemDtos
                   .Where(x => x.PlayerId == 2)
                   .OrderByDescending(x => x.Slot)
                   .Select(x => (uint)(x.Slot + 1))
                   .SingleOrDefaultAsync();

                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }
    }
}