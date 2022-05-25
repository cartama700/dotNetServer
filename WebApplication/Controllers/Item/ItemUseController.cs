using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Share.Protocol.API.Item.Use;
using API.Di;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Share.Structure;
using System.Collections.Generic;
using ServerLib.Database.Mysql.Dto.User;

namespace API.Controllers.Item
{
    /// <summary>
    /// 아이템 사용(수정) 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
            var itemIds = request.ItemStructureList.Select(x => x.Id).ToList();

            var userItemAndMasterDtoList = await (
                from UserItem in dbContext.UserItemDtos
                join MasterItem in dbContext.MasterItemDtos
                    on UserItem.ItemId equals MasterItem.Id
                where itemIds.Contains(MasterItem.Id) && UserItem.PlayerId == _playerId
                select new
                {
                    UserItem = new UserItemDto
                    {
                        Id = UserItem.Id,
                        ItemId = UserItem.ItemId,
                        Count = UserItem.Count,
                    },
                    MasterItem
                }
            ).ToListAsync();

            var cashStructureList = new List<CashStructure>();
            foreach (var itemStructure in request.ItemStructureList)
            {
                var userItemAndMasterDto = userItemAndMasterDtoList.SingleOrDefault(x => x.UserItem.ItemId == itemStructure.Id);
                if (userItemAndMasterDto == null)
                {
                    throw new ArgumentException("존재 하지 않는 아이템");
                }

                if (userItemAndMasterDto.UserItem.Count < itemStructure.Count)
                {
                    throw new ArgumentException("소지 수량 부족");
                }

                var cashStructure = cashStructureList.SingleOrDefault(x => x.CashType == userItemAndMasterDto.MasterItem.CashType);
                if (cashStructure == null)
                {
                    cashStructure = new CashStructure
                    {
                        CashType = userItemAndMasterDto.MasterItem.CashType
                    };
                    cashStructureList.Add(cashStructure);
                }

                cashStructure.Count += (userItemAndMasterDto.MasterItem.Price * itemStructure.Count) / 2;

                userItemAndMasterDto.UserItem.Count -= itemStructure.Count;

                _totalDi._daoContext._userItemDao.Update(userItemAndMasterDto.UserItem);
            }

            var userCashDtoList = new List<UserCashDto>();

            foreach (var cashStructure in cashStructureList)
            {
                await _totalDi._daoContext._userCashDao.InsertOrUpdateAsync(new UserCashDto
                {
                    PlayerId = _playerId,
                    CashType = cashStructure.CashType,
                    Count = cashStructure.Count,
                });
            }

            await using(var transaction = await dbContext.Database.BeginTransactionAsync())
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
                CashStructuresList = cashStructureList
            });
        }
    }
}
