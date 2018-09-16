using HotelBase.DataAccess.Resource;
using HotelBase.DataAccess.System;
using HotelBase.Entity;
using HotelBase.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.Service
{
    /// <summary>
    /// 酒店业务逻辑
    /// </summary>
    public static class HotelBll
    {
        /// <summary>
        /// 酒店查询
        /// </summary>
        /// <param name="request"></param>
        public static BasePageResponse<HotelSearchResponse> GetList(HotelSearchRequest request)
        {
            var data = H_HotelInfoAccess.GetList(request);
            var response = new BasePageResponse<HotelSearchResponse>()
            {
                IsSuccess = data.IsSuccess,
                Total = data.Total,
                List = new List<HotelSearchResponse>()
            };
            data?.List?.ForEach(x =>
            {
                response.List.Add(new HotelSearchResponse
                {
                    Id = x.Id,
                    Name = x.HIName,
                    //SourceId = x.SSourceId,
                    //Source = Sys_BaseDictionaryAccess.GetDicModel(0, x.SSourceId)?.DName ?? string.Empty,
                    CityId = x.HICityId,
                    CityName = x.HICity,
                    ProvName = x.HIProvince,
                    ProvId = x.HIProvinceId,
                    Valid = x.HIIsValid,
                    SupplierName = string.Empty,
                    Source = string.Empty
                });
            });
            return response;
        }
    }
}
