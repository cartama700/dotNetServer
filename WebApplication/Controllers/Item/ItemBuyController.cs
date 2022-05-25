using System;
using Microsoft.AspNetCore.Mvc;
using Share.Protocol.API.Item.Buy;
using Share.Structure;
using System.Collections.Generic;
using Share.Type.Item;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using API.Di;
using ServerLib.Database.Mysql.Dto.User;

namespace API.Controllers.Item
{
    /// <summary>
    /// 아이템 획득(구매)
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ItemBuyController : BaseController
    {
        public ItemBuyController(
            TotalDi totalDi
        ) : base(totalDi)
        {
        }

        [HttpPost]
        [Route("Item")]
        public async Task<JsonResult> ItemAsync(ItemBuyProtocol.Request request)
        {
            var dbContext = _totalDi._mysqlDbContext;
            var itemIds = request.ItemStructureList.Select(x => x.Id).ToList();

            var masterItemDto = await dbContext.MasterItemDtos
                .Where(x => itemIds.Contains(x.Id))
                .ToListAsync();

            //요청 확인 및 마스터 데이터 확인
            if(request.ItemStructureList.Count != masterItemDto.Count())
            {
                throw new NotSupportedException("잘못된 요청입니다.");
            }

            var buyItemCashTypeList = masterItemDto.Select(x => x.CashType).ToList();

            var userCashDtoList =  await dbContext.UserCashDtos
                .Where(x => x.PlayerId == _playerId)
                .Where(x => buyItemCashTypeList.Contains(x.CashType))
                .ToListAsync();

            var totalConsumeCashDict = new Dictionary<CashType, uint>();
            var useCashTypeList = masterItemDto.Select(x => x.CashType).ToList();
            foreach(var cashType in useCashTypeList)
            {
                totalConsumeCashDict.Add(cashType, 0);
            }

            foreach (var itemStructure in request.ItemStructureList)
            {
                var itemMasterDto = masterItemDto.Where(x => x.Id == itemStructure.Id).SingleOrDefault();

                totalConsumeCashDict[itemMasterDto.CashType] += (itemMasterDto.Price * itemStructure.Count);
            }


            var cashStructureList = new List<CashStructure>();
            foreach (var userCashDto in userCashDtoList)
            {
                var cashType = userCashDto.CashType;
                if (userCashDto.Count < totalConsumeCashDict[cashType])
                {
                    throw new ArgumentException("소지금 부족");
                }

                userCashDto.Count -= totalConsumeCashDict[cashType];
                _totalDi._daoContext._userCashDao.Update(userCashDto);
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

                userItemDto = await _totalDi._daoContext._userItemDao.InsertOrUpdateAsync(userItemDto);
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
