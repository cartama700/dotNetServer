using System;
using Microsoft.AspNetCore.Mvc;
using Share.Protocol.API.Item.Get;
using Share.Structure;
using System.Collections.Generic;
using ServerLib.Utill;
using Share.Type.Item;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using API.Di;

namespace API.Controllers.Item
{
    [ApiController]
    [Route("[controller]")]
    public class ItemGetController : BaseController
    {
        public ItemGetController(
            TotalDi totalDi
        ) : base(totalDi)
        {
        }

        [HttpGet]
        [Route("Item")]
        public async Task<JsonResult> ItemAsync(string ids)
        {
            var dbContext = _totalDi._mysqlDbContext;

            var itemIds = ids.ValidationSplit(',').Select(UInt32.Parse).ToList();

            var itemStructureList = await (from UserItem in dbContext.UserItemDtos
                                           join MasterItem in dbContext.MasterItemDtos
                                               on UserItem.ItemId equals MasterItem.Id
                                           where itemIds.Contains(MasterItem.Id) && UserItem.PlayerId == _playerId
                                           select new ItemStructure
                                           {
                                               Type = MasterItem.ItemType,
                                               Id = MasterItem.Id,
                                               Count = UserItem.Count,
                                           }).ToListAsync();
            
            return Json(new ItemGetProtocol.Response
            {
                ItemDataList = itemStructureList
            });
        }

        [HttpGet]
        [Route("ItemGetByType")]
        public async Task<JsonResult> ItemByTypeAsync(int type)
        {
            ItemType itemType;
            if (Enum.IsDefined(typeof(ItemType), type))
            {
                itemType = (ItemType)type;
            }
            else
            {
                throw new ArgumentException("올바른 아이템 타입이 아닙니다.");
            }

            var itemStructureList = 
                await (from UserItem in _totalDi._mysqlDbContext.UserItemDtos
                    join MasterItem in _totalDi._mysqlDbContext.MasterItemDtos
                        on UserItem.ItemId equals MasterItem.Id
                    where MasterItem.ItemType == itemType && UserItem.PlayerId == _playerId
                    select new ItemStructure
                    {
                        Type = MasterItem.ItemType,
                        Id = MasterItem.Id,
                        Count = UserItem.Count,
                    }).ToListAsync();

            return Json(new ItemGetProtocol.Response
            {
                ItemDataList = itemStructureList
            });
        }
    }
}
