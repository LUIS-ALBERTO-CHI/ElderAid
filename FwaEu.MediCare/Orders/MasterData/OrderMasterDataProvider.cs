using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Patients.Services;
using FwaEu.MediCare.Referencials.Services;
using FwaEu.Modules.MasterData;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.MasterData
{
    public class OrderMasterDataProvider : IMasterDataProvider
    {
        public OrderMasterDataProvider(ICurrentDateTime currentDateTime, IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            if (!_dateTimeNow.HasValue)
            {
                _dateTimeNow = currentDateTime.Now;
            }
        }

        private readonly IOrderService _orderService;
        public Type IdType => typeof(string);

        private static DateTime? _dateTimeNow = null;

        public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
        {
            var count = (await _orderService.GetAllAsync()).Count;

            return await Task.FromResult(new MasterDataChangesInfo(_dateTimeNow, count));
        }

        public async Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
        {
            if (parameters.Search != null)
            {
                throw new NotSupportedException("Search is not supported by building master-data.");
            }

            if (parameters.Pagination != null)
            {
                throw new NotSupportedException("Pagination is not supported by building master-data.");
            }

            if (parameters.OrderBy != null)
            {
                throw new NotSupportedException("OrderBy is not supported by building master-data.");
            }
            return await _orderService.GetAllAsync();
        }

        public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
        {
            throw new NotSupportedException(); // NOTE: It's a small master-data, pagination is not useful
        }
    }
}