using System.Collections.Generic;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class DeliveryBusiness : IDeliveryBusiness
    {
		public List<DeliveryDto> GetDeliveriesByFilter(FilterDeliveryDto filterDeliveryDto)
		{
			if (filterDeliveryDto != null)
			{
				var filterDelivery = filterDeliveryDto.CreateEntity();
				var deliveries = _deliveryRepository.GetDeliveriesByFilter(filterDelivery);
				var deliveriesDto = deliveries.CreateDto();
				return deliveriesDto;
			}
			else
			{
				return null;
			}
		}

		public List<StatusDeliveryDto> GetStatusDeliveries()
		{
			return _deliveryRepository.GetStatusDeliveries()?.CreateDto();
		}

		public DeliveryBusiness(IDeliveryRepository deliveryRepository)
		{
			_deliveryRepository = deliveryRepository;				   
		}

		private readonly IDeliveryRepository _deliveryRepository;
	}
}
