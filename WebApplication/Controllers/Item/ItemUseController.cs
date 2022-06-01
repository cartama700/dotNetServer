using API.Di;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Dto.Master.Item;
using ServerLib.Database.Mysql.Dto.User;
using Share.Protocol.API.Item.Use;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Item
{
    /// <summary>
    /// 아이템 사용(수정) 
    /// </summary>
    [ApiController]
    [Route("Item")]
    public class ItemUseController : BaseController
    {
        public ItemUseController(
            TotalDi totalDi
        ) : base(totalDi)
        {
        }

        /// <summary>
        /// 아이템 판매 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPut]
        [Route("Item")]
        public async Task<JsonResult> ItemSellAsync(ItemSellProtocol.Request request)
        {
            var dbContext = _totalDi._mysqlDbContext;
            var itemIds = request.ItemStructureList.GroupBy(x => x.Id).Select(x => x.Key).ToList();

            var masterItemDtoList = _totalDi._masterCache.Get<MasterItemDto>(itemIds);

            var userItemDtoDict = await dbContext.UserItemDtos
                .Where(x => x.PlayerId == _playerId)
                .Where(x => itemIds.Contains(x.ItemId))
                .Select(x => new UserItemDto
                {
                    Id = x.ItemId,
                    ItemId = x.ItemId,
                    Count = x.Count
                })
                .ToDictionaryAsync(x => x.ItemId);

            if (itemIds.Count != userItemDtoDict.Count)
            {
                throw new ArgumentException("소지하고 있지 않은 아이템 입니다.");
            }

            dbContext.AttachRange(userItemDtoDict.Select(x => x.Value));

            var useCashTypes = masterItemDtoList.GroupBy(x => x.CashType).Select(x => x.Key).ToList();

            var userCashDtoDict = await dbContext.UserCashDtos
                .Where(x => x.PlayerId == _playerId)
                .Where(x => useCashTypes.Contains(x.CashType))
                .Select(x => new UserCashDto
                {
                    Id = x.Id,
                    CashType = x.CashType,
                    Count = x.Count,
                })
                .ToDictionaryAsync(x => x.CashType);

            dbContext.AttachRange(userCashDtoDict.Select(x => x.Value));

            foreach (var itemStructure in request.ItemStructureList)
            {
                var masterItemDto = masterItemDtoList.SingleOrDefault(x => x.Id == itemStructure.Id);
                userItemDtoDict.TryGetValue(itemStructure.Id, out var userItemDto);

                if (userItemDto.Count < itemStructure.Count)
                {
                    throw new ArgumentException("소지수량 부족");
                }

                var giveCashCount = (masterItemDto.Price * itemStructure.Count) / 2;

                userCashDtoDict.TryGetValue(masterItemDto.CashType, out var userCashDto);
                if (userCashDto == null)
                {
                    var result = await dbContext.UserCashDtos.AddAsync(new UserCashDto
                    {
                        PlayerId = _playerId,
                        CashType = masterItemDto.CashType,
                        Count = 0,
                    });

                    userCashDto = result.Entity;

                    dbContext.Attach(userCashDto);
                    userCashDtoDict.Add(userCashDto.CashType, userCashDto);
                }

                userCashDto.Count += giveCashCount;

                userItemDto.Count -= itemStructure.Count;
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

            return Json(new ItemSellProtocol.Response
            {
                CashStructuresList = userCashDtoDict.Select(x => x.Value.ToCashStructure()).ToList()
            });
        }
    }
}
