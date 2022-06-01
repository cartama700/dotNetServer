using API.Di;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.Master.Item;
using ServerLib.Database.Mysql.Dto.User;
using ServerLib.Utill;
using Share.Protocol.API.Item.Get;
using Share.Structure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Item
{
    /// <summary>
    /// 소유 중인 아이템 정보를 가져오기
    /// </summary>
    [ApiController]
    [Route("Item")]
    public class ItemGetController : BaseController
    {
        public ItemGetController(
            TotalDi totalDi
        ) : base(totalDi)
        {
        }

        /// <summary>
        /// 보유 중인 아이템 정보 가져오기
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ItemGetProtocolResponse> ItemAsync(string ids)
        {
            var dbContext = _totalDi._mysqlDbContext;

            var itemIds = ids.ValidationSplit(',').Select(uint.Parse).ToList();

            var masterItemDtoList = _totalDi._masterCache.Get<MasterItemDto>(itemIds);

            var userItemDtoList = await dbContext.UserItemDtos
                .AsNoTracking()
                .Where(x => x.PlayerId == _playerId)
                .Where(x => itemIds.Contains(x.ItemId))
                .Select(x => new UserItemDto
                {
                    ItemId = x.ItemId,
                    Count = x.Count,
                })
                .ToListAsync();

            // 유저 아이템정보와 마스터 데이터를 종합
            var itemStructureList = (
                from userItemDto in userItemDtoList
                join masterItemDto in masterItemDtoList
                    on userItemDto.ItemId equals masterItemDto.Id
                select new ItemStructure
                {
                    Type = masterItemDto.ItemType,
                    Id = masterItemDto.Id,
                    Count = userItemDto.Count,
                }
            ).ToList();

            return new ItemGetProtocolResponse
            {
                ItemDataList = itemStructureList
            };
        }
    }
}
