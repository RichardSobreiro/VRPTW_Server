using System.Collections.Generic;
using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface IDeliveryBusiness
    {
		void ScheduleDeliveries(List<DeliveryDto> deliveriesTobeScheduled);
		List<DeliveryDto> GetDeliveriesByFilter(FilterDeliveryDto filterDeliveryDto);
	}
}
