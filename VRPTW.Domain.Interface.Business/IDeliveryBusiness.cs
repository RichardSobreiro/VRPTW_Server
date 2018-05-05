using System.Collections.Generic;
using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface IDeliveryBusiness
    {																		 
		List<DeliveryDto> GetDeliveriesByFilter(FilterDeliveryDto filterDeliveryDto);
		List<StatusDeliveryDto> GetStatusDeliveries();
	}
}
