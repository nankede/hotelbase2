﻿using HotelBase.Entity;
using HotelBase.Entity.Models;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBase.DataAccess.Resource;
using HotelBase.Common;

namespace HotelBase.Service
{
    /// <summary>
    /// 价格政策
    /// </summary>
    public class HotelPriceBll
    {
        /// <summary>
        /// 价格查询
        /// </summary>
        /// <param name="request"></param>
        public static List<HotelPriceModel> GetList(HotelPriceSearchRequest request)
        {
            var db = new H_HoteRulePriceAccess();
            var query = db.Query().Where(x => x.HRRId == request.RuleId)
                .Where(x => x.HRPDateInt > request.Month * 100 && x.HRPDateInt < (request.Month + 1) * 100)
                .OrderByDescending(x => x.Id);
            var list = query.ToList();

            var response = list?.Select(x => new HotelPriceModel
            {
                Id = x.Id,
                ContractPrice = x.HRPContractPrice,
                Count = x.HRPCount,
                PriceDate = x.HRPDate.ToString("yyyy-MM-dd"),
                RetainCount = x.HRPRetainCount,
                SellPrice = x.HRPSellPrice

            })?.ToList();
            return response;
        }

        /// <summary>
        /// 查询酒店房型详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static H_HoteRulePriceModel GetDetail(int id)
        {
            var model = new H_HoteRulePriceAccess().Query().FirstOrDefault(x => x.Id == id);
            return model;
        }

        /// <summary>
        /// 新增酒店价格详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Insert(H_HoteRulePriceModel model)
        {
            var res = new BaseResponse();

            var id = new H_HoteRulePriceAccess().Add(model);
            if (id <= 0)
            {
                res.Msg = "新增失败";
                return res;
            }
            else
            {
                res = new BaseResponse
                {
                    AddId = (int)id,
                    IsSuccess = 1
                };
            }
            return res;
        }

        /// <summary>
        /// 修改酒店价格详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BaseResponse Update(H_HoteRulePriceModel model)
        {
            var res = new BaseResponse();
            if (model.Id <= 0)
            {
                res.Msg = "无效的价格";
                return res;
            }
            var i = new H_HoteRulePriceAccess().Update(model);
            res = new BaseResponse
            {
                IsSuccess = i ? 1 : 0,
                Msg = i ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 设置有效性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BaseResponse SetValid(int id, int valid, string name)
        {
            var i = new H_HoteRulePriceAccess().Update().Where(x => x.Id == id)
                .Set(x => x.HRPIsValid == valid && x.HRPUpdateName == name && x.HRPUpdateTime == DateTime.Now)
                .Execute();
            var res = new BaseResponse
            {
                IsSuccess = i > 0 ? 1 : 0,
                Msg = i > 0 ? string.Empty : "更新失败",
            };
            return res;
        }

        /// <summary>
        /// 酒店详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BaseResponse SavePriceDetail(SaveHotelPriceModel request)
        {
            //验证对于日期是否存在
            var db = new H_HoteRulePriceAccess();
            var model = new H_HoteRulePriceModel();
            var date = ConvertHelper.ToDateTime(request.PriceDate, new DateTime(1900, 1, 1));
            var dateInt = ConvertHelper.ToInt32(date.ToString("yyyyMMdd"), 0);
            if (date.Year < 2000)
            {
                return new BaseResponse { Msg = "日期错误" };
            }

            model = db.Query().Where(x => x.HRPDateInt == dateInt).FirstOrDefault();
            if (model != null && model.Id > 0)
            {
                if (model.Id != request.Id)
                {
                    return new BaseResponse { Msg = "数据有问题" };
                }
                model.HRPUpdateName = request.OperateName;
                model.HRPUpdateTime = DateTime.Now;
            }
            else
            {
                model = new H_HoteRulePriceModel
                {
                    HRPDate = date,
                    HRPDateInt = dateInt,
                    HRPAddName = request.OperateName,
                    HIId = request.HotelId,
                    HRId = request.RoomId,
                    HRRId = request.RuleId,
                    HRPAddTime = DateTime.Now,
                    HRPStatus = 1,
                };
            }

            if (request.Type == 1)
            {//价格
                model.HRPSellPrice = request.SellPrice;
                model.HRPContractPrice = request.ContractPrice;
            }
            if (request.Type == 2)
            {// 库存
                model.HRPCount = request.Count;
                model.HRPRetainCount = request.RetainCount;
            }
            if (model.Id > 0)
            {
                return Update(model);
            }
            else
            {
                return Insert(model);
            }
        }
    }
}
