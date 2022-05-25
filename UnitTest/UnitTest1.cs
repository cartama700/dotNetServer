using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            using (var context = new MysqlDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var result = await (from UserItem in context.UserItemDtos
                                    join MasterItem in context.MasterItemDtos on UserItem.ItemId equals MasterItem.Id
                where UserItem.PlayerId == 1
                select new
                {
                    UserItem =  new UserItemDto
                    {
                        Id = UserItem.Id,
                        ItemId = UserItem.ItemId,
                        Count = UserItem.Count,
                    },
                    MasterItem
                }).ToListAsync();


                var ass = result
                    .Where(x => x.UserItem != null)
                    .SingleOrDefault(x => x.UserItem.ItemId == 2);
                if(ass != null)
                {
                    ass.UserItem.Count += 1;
                    ass.UserItem.ItemId = 3;
                    var entry = context.Entry(ass.UserItem);
                    entry.Property(x => x.Count).IsModified = true;

                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}