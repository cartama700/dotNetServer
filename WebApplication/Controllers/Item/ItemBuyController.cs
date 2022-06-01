using API.Di;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.Master.Item;
using ServerLib.Database.Mysql.Dto.User;
using Share.Protocol.API.Item.Buy;
using Share.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Item
{
    /// <summary>
    /// 아이템 생성 관련
    /// </summary>
    [ApiController]
    [Route("Item")]
    public class ItemBuyController : BaseController
    {
        public ItemBuyController(
            TotalDi totalDi
        ) : base(totalDi)
        {
        }

        /// <summary>
        /// 아이템 구매
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<JsonResult> ItemAsync(ItemBuyProtocol.Request request)
        {
            var dbContext = _totalDi._mysqlDbContext;
            var itemIds = request.ItemStructureList.Select(x => x.Id).ToList();

            var masterItemDtoList = _totalDi._masterCache.Get<MasterItemDto>(itemIds);

            var useItemCashTypeList = masterItemDtoList.Select(x => x.CashType).ToList();

            var userCashDtoList = await dbContext.UserCashDtos
                .Where(x => x.PlayerId == _playerId)
                .Where(x => useItemCashTypeList.Contains(x.CashType))
                .ToListAsync();

            dbContext.AttachRange(userCashDtoList);

            var cashStructureList = new List<CashStructure>();

            //사용되는 캐쉬의 총합
            var totalCashSums =
                from buyList in request.ItemStructureList
                join masterItemDto in masterItemDtoList on buyList.Id equals masterItemDto.Id
                group masterItemDto by masterItemDto.CashType into g
                select new
                {
                    CashType = g.Key,
                    Price = (uint)g.Sum(x => x.Price)
                };

            foreach (var userCashDto in userCashDtoList)
            {
                var totalCashSum = totalCashSums.SingleOrDefault(x => x.CashType == userCashDto.CashType);

                if (userCashDto.Count < totalCashSum.Price)
                {
                    throw new ArgumentException("소지금 부족");
                }

                userCashDto.Count -= totalCashSum.Price;

                cashStructureList.Add(userCashDto.ToCashStructure());
            }

            var itemStructureList = new List<ItemStructure>();
            foreach (var itemStructure in request.ItemStructureList)
            {
                var userItemDto = new UserItemDto
                {
                    PlayerId = _playerId,
                    ItemId = itemStructure.Id,
                    Count = itemStructure.Count,
                };

                userItemDto = await _totalDi._daoContext._userItemDao.InsertOrUpdateAsync(userItemDto, await _totalDi._playerDi.GetLastSlotAsync(true));
                itemStructureList.Add(userItemDto.ToItemStructure(itemStructure.Type));
            }

            await using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await transaction.CommitAsync();
                }
                catch (OperationCanceledException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

            return Json(new ItemBuyProtocol.Response
            {
                ItemStructureList = itemStructureList,
                CashStructureList = cashStructureList
            });
        }
    }
}
